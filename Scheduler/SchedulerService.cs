using TestMySql.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntryManagement.Service
{
   
    public class SchedulerService
    {
        public async Task StartScheduler( int hour1 = 12  , int miute1 = 0 , int hour2 = 21 , int miute2 = 55)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            await scheduler.Start();

            // Job 1: PhatCanhBaoJob for group 1
            IJobDetail job1 = JobBuilder.Create<PhatCanhBaoJob>()
                .WithIdentity("PhatCanhBaoJobNhom1", "Nhom1") // Job name and group
                .Build();

            ITrigger trigger1 = TriggerBuilder.Create()
                .WithIdentity("TriggerPhatCanhBaoNhom1", "Nhom1") // Trigger name and group
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour1, miute1)) // Schedule at 23:15 daily
                .Build();

            // Job 2: PhatCanhBaoJob for group 2
            IJobDetail job2 = JobBuilder.Create<PhatCanhBaoJob>()
                .WithIdentity("PhatCanhBaoJobNhom2", "Nhom2") // Job name and group
                .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("TriggerPhatCanhBaoNhom2", "Nhom2") // Trigger name and group
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour2, miute2)) // Schedule at 20:55 daily
                .Build();

            // Schedule job 1 and job 2 with respective triggers
            await scheduler.ScheduleJob(job1, trigger1);
            await scheduler.ScheduleJob(job2, trigger2);
        }

        public class PhatCanhBaoJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                MailService mailService = new MailService();
                SoundService soundService = new SoundService();

                using (EntrylogmanagementContext _context = new EntrylogmanagementContext())
                {
                    var entriesByDay = await _context.Entrylogs
                        .Include(e => e.Student)
                        .Where(e => e.LogTime.Date == DateTime.Now.Date) // Filter by specific date
                        .GroupBy(e => new
                        {
                            e.Student.Name,
                            e.Student.StudentId,
                            ParentEmail = e.Student.Parent.Email // If Parent is not in EntryLog, it will return null
                        })
                        .Select(g => new
                        {
                            StudentName = g.Key.Name,
                            ParentEmail = g.Key.ParentEmail,
                            StudentId = g.Key.StudentId,
                            EntryCount = g.Count()
                        })
                        .ToListAsync();

                    Console.WriteLine("Entry log information for the day:");
                    foreach (var entry in entriesByDay)
                    {
                        if ((entry.EntryCount % 2) != 0)
                        {
                            mailService.SendEmail(entry.StudentName );
                            Console.WriteLine($"Student name: {entry.StudentName}");
                            Console.WriteLine($"Parent email: {entry.ParentEmail ?? "No information available"}"); // Handle when ParentEmail is null
                            Console.WriteLine($"Entry count: {entry.EntryCount}");
                            Console.WriteLine("---------------------------------------");
                            try
                            {
                                Alert alert = new Alert()
                                {
                                    StudentId = entry.StudentId,
                                    AlertTime = DateTime.Now 
                                };

                                _context.Alerts.Add(alert);
                                await _context.SaveChangesAsync();
                                Console.WriteLine("Saved successfully");
                            }catch(Exception ex)
                            {
                                Console.WriteLine("Loi them vao database " + ex.InnerException.Message);                            }
                         
                        }
                    }

                    soundService.PlaySoundLog();
                }

                await Task.CompletedTask;
            }
        }
    }
}
