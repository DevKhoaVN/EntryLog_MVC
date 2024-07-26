using AForge.Video;
using AForge.Video.DirectShow;

using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TestMySql.Models;
using ZXing;
using ZXing.Windows.Compatibility;

namespace EntryManagement.Service
{
    internal class Camera
    {
        private static FilterInfoCollection videoDevices;
        private static VideoCaptureDevice videoSource;
        private static bool isRunning = true;
        private static List<Entrylog> entryLogs = new List<Entrylog>();

        public void TurnOn()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                Console.WriteLine("No video input devices found.");
                return;
            }

            // Use the first video device found
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();


           
            Console.ReadLine();

            // Stop the video source
            videoSource.SignalToStop();
            videoSource.WaitForStop();
        }

        private static async void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!isRunning)
                return;

            using (EntrylogmanagementContext context = new EntrylogmanagementContext())
            {
                try
                {
                    // Convert AForge's VideoFrame to Bitmap
                    Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                    // Decode QR code from bitmap
                    var reader = new BarcodeReader();
                    var result = reader.Decode(bitmap);

                    if (result != null && int.TryParse(result.Text, out int StudentID))
                    {
                        // Check if student with the given ID exists
                        var student = context.Students.FirstOrDefault(f => f.StudentId == StudentID);

                        if (student != null)
                        {
                            var status = "Int"; // Default status if no previous log is found

                            // Get the latest entry log for the student
                            var entry = context.Entrylogs.Where(e => e.StudentId == StudentID)
                                                         .OrderByDescending(e => e.LogTime)
                                                         .FirstOrDefault();

                            // Check if there's a recent log within 1 minute
                            if (entry != null && (DateTime.Now - entry.LogTime).TotalSeconds < 5)
                            {
                                return;
                            }

                            // Determine log status based on previous log
                            if (entry != null && entry.Status == "Int")
                            {
                                status = "Out";
                            }
                            else
                            {
                                status = "Int";
                            }

                            var time = DateTime.Now;
                            // Create new entry log
                            Entrylog log = new Entrylog
                            {
                                LogTime = time,
                                StudentId = StudentID,
                                Status = status
                            };

                             entryLogs.Add(log);

                            // Render the table
                            RenderTable(context);

                            // Add new log to context and save changes
                            context.Entrylogs.Add(log);
                            await context.SaveChangesAsync();

                          

                            // Play sound
                            SoundService sound = new SoundService();
                            sound.PlaySoundCamera();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while decoding QR code: {ex.Message}");
                }
            }
        }

        private static void RenderTable(EntrylogmanagementContext context)
        {
            var table = new Table().Expand();
            table.Title("[yellow]Bảng ra vào[/]");
            table.AddColumn("ID");
            table.AddColumn("Học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian");
            table.AddColumn("Trạng thái");

            foreach (var log in entryLogs)
            {
                var student = context.Students.FirstOrDefault(s => s.StudentId == log.StudentId);
                if (student != null)
                {
                    table.AddRow($"{student.StudentId}",
                                 $"{student.Name}",
                                 $"{student.Class}",
                                 $"{log.LogTime}",
                                 $"{log.Status}");
                }
            }

            AnsiConsole.Clear();
            AnsiConsole.Render(table);
        }
    }
}
