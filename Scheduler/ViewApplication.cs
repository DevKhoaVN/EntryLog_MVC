using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Identity.Client;
using MySqlX.XDevAPI.Common;
using TestMySql.Models;
using Google.Protobuf.WellKnownTypes;
using System.Text.RegularExpressions;

namespace TestMySql
{
    public class ViewApplication
    {
        public  int GetValidHour(string prompt)
        {
            int hour;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                if (int.TryParse(Console.ReadLine(), out hour) && hour >= 0 && hour <= 23)
                {
                    break;
                }
                Console.WriteLine("[red]Giờ không hợp lệ. Vui lòng nhập giá trị từ 0 đến 23.[/]");
            }
            return hour;
        }

        public int GetValidMinute(string prompt)
        {
            int minute;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                if (int.TryParse(Console.ReadLine(), out minute) && minute >= 0 && minute <= 59)
                {
                    break;
                }
                AnsiConsole.MarkupLine("[red]Phút không hợp lệ. Vui lòng nhập giá trị từ 0 đến 59.[/]");
            }
            return minute;
        }
        public string GetValidEmail(string prompt)
        {
            string email;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Simple email validation pattern

            // Loop until a valid email is entered
            while (true)
            {
                // Display the prompt to the user
                AnsiConsole.Markup(prompt);

                // Get the user's input with a green prompt style
                email = AnsiConsole.Prompt(
                    new TextPrompt<string>("")
                        .PromptStyle("yellow"));

                // Validate the email using regex
                if (Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
                {
                    break; // Exit the loop if the email is valid
                }
                else
                {
                    // Display an error message if the email is invalid
                    AnsiConsole.MarkupLine("[red]Invalid email address! Please enter a valid email.[/]");
                }
            }

            return email; // Return the valid email
        }

        public  int GetValidPhoneNumber(string prompt)
        {
            string input;
            string phoneNumberPattern = @"^\d{10}$"; // Pattern for exactly 10 digits

            // Loop until a valid phone number is entered
            while (true)
            {
                // Display the prompt to the user
                AnsiConsole.Markup(prompt);

                // Get the user's input with a green prompt style
                input = AnsiConsole.Prompt(
                    new TextPrompt<string>("")
                        .PromptStyle("yellow"));

                // Validate the input using regex
                if (Regex.IsMatch(input, phoneNumberPattern))
                {
                    // Convert valid input to integer
                    if (int.TryParse(input, out int phoneNumber))
                    {
                        return phoneNumber; // Return the valid phone number as int
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Failed to convert phone number to an integer.[/]");
                    }
                }
                else
                {
                    // Display an error message if the phone number is invalid
                    AnsiConsole.MarkupLine("[red]Invalid phone number! Please enter a 10-digit phone number.[/]");
                }
            }
        }

        public string GetStringPrompt(string prompt)
        {
            
                AnsiConsole.Markup(prompt);

                string input = AnsiConsole.Prompt(
                    new TextPrompt<string>("")
                        .PromptStyle("yellow"));
            

            return input;
        }

        public int GetIntPrompt(string prompt)
        {
            int result = 0;
            bool isValid = false;

            while (!isValid)
            {
                // Hiển thị thông báo và nhận đầu vào dưới dạng chuỗi
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>(prompt)
                        .PromptStyle("yellow")
                        .ValidationErrorMessage("Vui lòng nhập một số nguyên hợp lệ.")
                );

                // Kiểm tra xem đầu vào có phải là số nguyên không
                isValid = int.TryParse(input, out result);

                // Nếu đầu vào không hợp lệ, hiển thị thông báo lỗi
                if (!isValid)
                {
                    AnsiConsole.MarkupLine("[red]Đầu vào không phải là số nguyên. Vui lòng thử lại.[/]");
                }
            }

            return result;
        }


        public string GetPasswordPrompt(string Prompt)
        {
          

            return AnsiConsole.Prompt(
                new TextPrompt<string>(Prompt)
                    .PromptStyle("red")
                    .Secret());
        }

        public void TextReuslt(string Prompt)
        {
            AnsiConsole.MarkupLine(Prompt);
            AnsiConsole.WriteLine();
        }

        public DateTime GetDate(string prompt)
        {
            string[] formats = new[]
            {
            "dd/MM/yyyy HH:mm",  // Ví dụ: 25/12/2024 14:30
            "MM/dd/yyyy HH:mm",  // Ví dụ: 12/25/2024 14:30
            "yyyy-MM-dd HH:mm",  // Ví dụ: 2024-12-25 14:30
            "dd-MM-yyyy HH:mm",  // Ví dụ: 25-12-2024 14:30
            "yyyy/MM/dd HH:mm",  // Ví dụ: 2024/12/25 14:30
            "dd/MM/yyyy",        // Ví dụ: 25/12/2024
            "MM/dd/yyyy",        // Ví dụ: 12/25/2024
            "yyyy-MM-dd",        // Ví dụ: 2024-12-25
            "dd-MM-yyyy"         // Ví dụ: 25-12-2024
             };

            while (true)
            {
                AnsiConsole.Markup(prompt); // Hiển thị lời nhắc cho người dùng
                string input = Console.ReadLine(); // Đọc dữ liệu đầu vào

                // Thử chuyển đổi đầu vào thành kiểu DateTime với các định dạng khác nhau
                if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date; // Trả về giá trị DateTime nếu chuyển đổi thành công
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Định dạng ngày giờ không hợp lệ. Vui lòng nhập lại theo định dạng sau: dd-MM-yyyy.[/]");
                    AnsiConsole.WriteLine();
                }
            }
        }

        // Phương thức cập nhật thông tin phụ huynh
        public Parent UpdateParent(Parent parent)
        {
            // Nhập tên mới cho phụ huynh (nếu muốn thay đổi)
            string newName = AnsiConsole.Ask<string>("Nhập [green]tên phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Name);
            if (!string.IsNullOrEmpty(newName)) parent.Name = newName;

            // Nhập số điện thoại mới cho phụ huynh (nếu muốn thay đổi)
            string newPhoneString = AnsiConsole.Ask<string>("Nhập [green]số điện thoại phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Phone.ToString());
            if (!string.IsNullOrEmpty(newPhoneString) && int.TryParse(newPhoneString, out int newPhone) && newPhone > 0) parent.Phone = newPhone;

            // Nhập Email mới cho phụ huynh (nếu muốn thay đổi)
            string newEmail = AnsiConsole.Ask<string>("Nhập[green] Email phụ huynh mới (bỏ trống nếu không muốn thay đổi, phải là @gmail.com)[/]: ", parent.Email);
            if (!string.IsNullOrEmpty(newEmail) && newEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)) parent.Email = newEmail;

            // Nhập địa chỉ mới cho phụ huynh (nếu muốn thay đổi)
            string newAddress = AnsiConsole.Ask<string>("Nhập [green]địa chỉ phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Address);
            if (!string.IsNullOrEmpty(newAddress)) parent.Address = newAddress;

            return parent;
        }

        // Phương thức cập nhật thông tin học sinh
        public Student UpdateStudentInfo(Student student)
        {
            // Nhập tên mới cho học sinh (nếu muốn thay đổi)
            string newName = AnsiConsole.Ask<string>("Nhập [green]tên học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Name);
            if (!string.IsNullOrEmpty(newName)) student.Name = newName;

            // Nhập giới tính mới cho học sinh (nếu muốn thay đổi)
            string newGender = AnsiConsole.Ask<string>("Nhập [green]giới tính học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Gender);
            if (!string.IsNullOrEmpty(newGender)) student.Gender = newGender;

            // Nhập ngày sinh mới cho học sinh (nếu muốn thay đổi)
            string newDobString = AnsiConsole.Ask<string>("Nhập [green]ngày sinh học sinh mới (yyyy/MM/dd, bỏ trống nếu không muốn thay đổi)[/]: ", student.DayOfBirth.ToString("yyyy/MM/dd"));
            if (!string.IsNullOrEmpty(newDobString) && DateTime.TryParseExact(newDobString, "yyyy/MM/dd", null, DateTimeStyles.None, out DateTime newDob)) student.DayOfBirth = newDob;

            // Nhập lớp mới cho học sinh (nếu muốn thay đổi)
            string newClass = AnsiConsole.Ask<string>("Nhập [green]lớp học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Class);
            if (!string.IsNullOrEmpty(newClass)) student.Class = newClass;

            // Nhập địa chỉ mới cho học sinh (nếu muốn thay đổi)
            string newAddress = AnsiConsole.Ask<string>("Nhập [green] địa chỉ học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Address);
            if (!string.IsNullOrEmpty(newAddress)) student.Address = newAddress;

            // Nhập số điện thoại mới cho học sinh (nếu muốn thay đổi)
            string newPhoneString = AnsiConsole.Ask<string>("Nhập [green]số điện thoại học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Phone.ToString());
            if (!string.IsNullOrEmpty(newPhoneString) && int.TryParse(newPhoneString, out int newPhone) && newPhone > 0) student.Phone = newPhone;

            return student;
        }
        //==================================AbsentReport=========================================================
        public void AbsentReport_ShowAll(List<Absentreport> data)
        {
            // Tạo Bảng
            var table = new Table().Expand();

            // Thêm tiêu đề
            table.Title("[#ffff00]Bảng báo cáo vắng học[/]");

            //Thêm cột
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Lớp");
            table.AddColumn("Ngày báo cáo");
            table.AddColumn("Lý do");


            // thêm data vào hàng
            foreach (var report in data)
            {
                table.AddRow(
                    $"{report.StudentId}",
                    $"{report.Student.Name}",
                    $"{report.Parent.Name}",
                    $"{report.Student.Class}",
                    $"{report.CreateDay:yyyy-MM-dd}",
                    $"{report.Reason}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void AbsentReport_ID(List<Absentreport> data, int StudentID)
        {
            // Tạo Bảng
            var table = new Table().Expand();

            // Thêm tiêu đề
            table.Title("[#ffff00]Bảng báo cáo vắng học cho học sinh có ID {StudentId}[/]");

            //Thêm cột
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Lớp");
            table.AddColumn("Ngày báo cáo");
            table.AddColumn("Lý do");


            // thêm data vào hàng
            foreach (var report in data)
            {
                table.AddRow(
                    $"{report.StudentId}",
                    $"{report.Student.Name}",
                    $"{report.Parent.Name}",
                    $"{report.Student.Class}",
                    $"{report.CreateDay:yyyy-MM-dd}",
                    $"{report.Reason}"
                );
            }


            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void AbsentReport_RangeTime(List<Absentreport> data, DateTime timeStart, DateTime timeEnd)
        {
            // Tạo Bảng
            var table = new Table().Expand();

            // Thêm tiêu đề
            table.Title($"[#ffff00]Bảng báo cáo vắng học từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd} [/]");

            //Thêm cột
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Lớp");
            table.AddColumn("Ngày báo cáo");
            table.AddColumn("Lý do");


            // thêm data vào hàng
            foreach (var report in data)
            {
                table.AddRow(
                    $"{report.StudentId}",
                    $"{report.Student.Name}",
                    $"{report.Parent.Name}",
                    $"{report.Student.Class}",
                    $"{report.CreateDay:yyyy-MM-dd}",
                    $"{report.Reason}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        //==================================Alert=========================================================

        public void Alert_ID(List<Alert> alerts, int StudentId)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách cảnh báo cho học sinh có ID {StudentId}[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian cảnh báo");
            // Thêm các hàng vào bảng
            foreach (var alert in alerts)
            {
                table.AddRow(
                    $"{alert.StudentId}",
                    $"{alert.Student.Name}",
                    $"{alert.Student.Class}",
                    $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                    );
            }


            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void Alert_ShowAll(List<Alert> alerts)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách cảnh báo[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian cảnh báo");

            // Thêm các hàng vào bảng
            foreach (var alert in alerts)
            {
                table.AddRow(
                    $"{alert.StudentId}",
                    $"{alert.Student.Name}",
                    $"{alert.Student.Class}",
                    $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                    );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void Alert_RangeTime(List<Alert> alerts, DateTime timeStart, DateTime timeEnd)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách cảnh báo từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian cảnh báo");

            // Thêm các hàng vào bảng
            foreach (var alert in alerts)
            {
                table.AddRow(
                    $"{alert.StudentId}",
                    $"{alert.Student.Name}",
                    $"{alert.Student.Class}",
                    $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                    );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        //==================================EntryLog=========================================================

        public void Entrylog_ID(List<Entrylog> entryLogs, int? studentId)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách các bản ghi ra vào cho học sinh có ID {studentId}[/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian bản ghi");
            table.AddColumn("Trạng thái");

            // Thêm các hàng vào bảng
            foreach (var log in entryLogs)
            {
                table.AddRow(
                    $"{log.StudentId}",
                    $"{log.Student.Name}",
                    $"{log.Student.Class}",
                    $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                    $"{log.Status}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }


        public void Entrylog_ShowAll(List<Entrylog> entryLogs)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách các bản ghi ra vào[/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian bản ghi");
            table.AddColumn("Trạng thái");

            // Thêm các hàng vào bảng
            foreach (var log in entryLogs)
            {
                table.AddRow(
                    $"{log.StudentId}",
                    $"{log.Student.Name}",
                    $"{log.Student.Class}",
                    $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                    $"{log.Status}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void Entrylog_RangeTime(List<Entrylog> entryLogs, DateTime timeStart, DateTime timeEnd)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách các bản ghi ra vàotừ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Thời gian bản ghi");
            table.AddColumn("Trạng thái");

            // Thêm các hàng vào bảng
            foreach (var log in entryLogs)
            {
                table.AddRow(
                    $"{log.StudentId}",
                    $"{log.Student.Name}",
                    $"{log.Student.Class}",
                    $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                    $"{log.Status}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        //==================================studentInformation=========================================================


        public void StudentInfor_ID(List<Student> StudentInfor, int studentId)
        {

            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title("[#ffff00]Danh sách học sinh [/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Giới tính");
            table.AddColumn("Ngày sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Địa chỉ");
            table.AddColumn("Số điện thoại");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Email phụ huynh");
            table.AddColumn("Số điện thoại phụ huynh");
            table.AddColumn("Địa chỉ phụ huynh");

            // Thêm dữ liệu vào hàng
            foreach (var student in StudentInfor)
            {
                table.AddRow(
                    $"{student.StudentId}",
                    $"{student.Name}",
                    $"{student.Gender}",
                    $"{student.DayOfBirth:yyyy-MM-dd}",
                    $"{student.Class}",
                    $"{student.Address}",
                    $"{student.Phone}",
                    $"{student.Parent.Name}",
                    $"{student.Parent.Email}",
                    $"{student.Parent.Phone}",
                    $"{student.Parent.Address}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }


        public void StudentInfor_ShowAll(List<Student> StudentInfor)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title("[#ffff00]Danh sách thông tin học sinh[/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Giới tính");
            table.AddColumn("Ngày sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Địa chỉ");
            table.AddColumn("Số điện thoại");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Email phụ huynh");
            table.AddColumn("Số điện thoại phụ huynh");
            table.AddColumn("Địa chỉ phụ huynh");

            // Thêm dữ liệu vào hàng
            foreach (var student in StudentInfor)
            {
                table.AddRow(
                    $"{student.StudentId}",
                    $"{student.Name}",
                    $"{student.Gender}",
                    $"{student.DayOfBirth:yyyy-MM-dd}",
                    $"{student.Class}",
                    $"{student.Address}",
                    $"{student.Phone}",
                    $"{student.Parent.Name}",
                    $"{student.Parent.Email}",
                    $"{student.Parent.Phone}",
                    $"{student.Parent.Address}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        public void StudentInfor_RangeTime(List<Student> StudentInfor, DateTime timeStart, DateTime timeEnd)
        {
            // Tạo bảng và thêm các cột
            var table = new Table().Expand();
            table.Title($"[#ffff00]Danh sách thông tin học tham gia từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder();
            table.AddColumn("ID học sinh");
            table.AddColumn("Tên học sinh");
            table.AddColumn("Giới tính");
            table.AddColumn("Ngày sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Địa chỉ");
            table.AddColumn("Số điện thoại");
            table.AddColumn("Tên phụ huynh");
            table.AddColumn("Email phụ huynh");
            table.AddColumn("Số điện thoại phụ huynh");
            table.AddColumn("Địa chỉ phụ huynh");

            // Thêm dữ liệu vào hàng
            foreach (var student in StudentInfor)
            {
                table.AddRow(
                    $"{student.StudentId}",
                    $"{student.Name}",
                    $"{student.Gender}",
                    $"{student.DayOfBirth:yyyy-MM-dd}",
                    $"{student.Class}",
                    $"{student.Address}",
                    $"{student.Phone}",
                    $"{student.Parent.Name}",
                    $"{student.Parent.Email}",
                    $"{student.Parent.Phone}",
                    $"{student.Parent.Address}"
                );
            }

            // Hiển thị bảng
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }


        //==================================Entrylater=========================================================
        public void Entrylater_ID(List<Entrylog> studentLate, int studentId)
        {
            // Tạo bảng
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.Title("[#ffff00]Bảng học sinh đi muộn[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Trạng thái");
            table.AddColumn("Thời gian");

            if (studentLate != null && studentLate.Count > 0) // Ensure list is not empty
            {
                // Thêm dữ liệu vào bảng
                foreach (var student in studentLate)
                {
                    table.AddRow(
                       $"{student.StudentId}",
                       $"{student.Student.Name}",
                       $"{student.Student.Class}",
                       $"{student.Status}",
                       $"{student.LogTime}" // Format the time display
                   );
                }

                // Hiển thị bảng
                AnsiConsole.Render(table);
                AnsiConsole.WriteLine();
            }
        }


        public void Entrylater_ShowAll(List<Entrylog> studentLate)
        {
            // Tạo bảng
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.Title("[#ffff00]Bảng học sinh đi muộn[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Trạng thái");
            table.AddColumn("Thời gian");

            if (studentLate != null && studentLate.Count > 0) // Ensure list is not empty
            {
                // Thêm dữ liệu vào bảng
                foreach (var student in studentLate)
                {
                    table.AddRow(
                       $"{student.StudentId}",
                       $"{student.Student.Name}",
                       $"{student.Student.Class}",
                       $"{student.Status}",
                       $"{student.LogTime}" // Format the time display
                   );
                }

                // Hiển thị bảng
                AnsiConsole.Render(table);
                AnsiConsole.WriteLine();
            }
        }

        public void Entrylater_Today(List<Entrylog> studentLate)
        {
            // Tạo bảng
            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.Title("[#ffff00]Bảng học sinh đi muộn[/]").HeavyEdgeBorder();
            table.AddColumn("ID");
            table.AddColumn("Học sinh");
            table.AddColumn("Lớp");
            table.AddColumn("Trạng thái");
            table.AddColumn("Thời gian");

            if (studentLate != null && studentLate.Count > 0) // Ensure list is not empty
            {
                // Thêm dữ liệu vào bảng
                foreach (var student in studentLate)
                {
                    table.AddRow(
                       $"{student.StudentId}",
                       $"{student.Student.Name}",
                       $"{student.Student.Class}",
                       $"{student.Status}",
                       $"{student.LogTime}" // Format the time display
                   );
                }

                // Hiển thị bảng
                AnsiConsole.Render(table);
                AnsiConsole.WriteLine();
            }
        }

//==================================Entrylater=========================================================



    }
}

