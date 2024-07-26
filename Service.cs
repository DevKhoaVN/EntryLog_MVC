using EntryManagement.Service;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Quartz.Impl;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace TestMySql
{
    public class Service
    {
        private readonly EntrylogmanagementContext _context;
        private readonly SchedulerService schedulerService;
        private readonly ViewApplication view;
        private readonly DateTime today = DateTime.Today;
        private DateTime morningStartTime;
        private DateTime morningLateTime;
        private DateTime afternoonStartTime;
        private DateTime afternoonLateTime;

        public Service()
        {
            _context = new EntrylogmanagementContext();
            schedulerService = new SchedulerService();
            view = new ViewApplication();
            morningStartTime = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0); // 7:00 AM
            morningLateTime = new DateTime(today.Year, today.Month, today.Day, 7, 30, 0); // 7:30 AM

            afternoonStartTime = new DateTime(today.Year, today.Month, today.Day, 19, 0, 0); // 2:00 PM
            afternoonLateTime = new DateTime(today.Year, today.Month, today.Day, 19, 59, 0); // 2:30 PM


        }

        public bool Validate_User(string userName, string password, out int userRole, out int? StudentID)
        {
            // gán giá trị mặc định

            userRole = 0;
            StudentID = 0;

            // truy vấn lấy User có Username trùng với Username người dùng nhập vào
            var user = _context.Users.Where(e => e.UserName == userName).FirstOrDefault();


            // kiểm tra  username , password 
            if (user != null && user.Password == password)
            {
                userRole = user.RoleId;
                StudentID = user.StudentId;

                return true;
            }

            return false;
        }

        public bool HandleRegister(string userName, string password, string studentIdString)
        {
            // Validate the student ID input
            if (int.TryParse(studentIdString, out int studentId) && studentId > 0)
            {
                // Check if the student ID exists in the database
                var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);

                if (student != null)
                {
                    // Check if the username already exists
                    var existingUser = _context.Users.FirstOrDefault(x => x.UserName == userName);
                    if (existingUser != null)
                    {
                        AnsiConsole.MarkupLine("[red]Tên người dùng đã tồn tại. Vui lòng chọn tên khác![/]");
                        return false;
                    }

                    // Create a new user
                    var newUser = new User
                    {
                        UserName = userName,
                        Password = password, // Consider hashing this password
                        RoleId = 2, // Default role ID for standard users
                        StudentId = studentId
                    };

                    // Add and save the new user to the database
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    AnsiConsole.MarkupLine("[#00ff00]Đăng ký thành công![/]");
                    return true;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Học sinh không tồn tại trong hệ thống.[/]");
                    return false;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]ID không hợp lệ. Vui lòng nhập đúng ID học sinh![/]");
                return false;
            }
        }

        public List<Absentreport> AbsentReport_ID(int reportId)
        {
            // Lấy báo cáo vắng học theo ID và sắp xếp theo ngày tạo giảm dần
            var absentReport = _context.Absentreports
                .Include(e => e.Student)  // Nạp đối tượng Student
                .Include(e => e.Parent)   // Nạp đối tượng Parent
                .Where(e => e.AbsentReportId == reportId)
                .OrderByDescending(e => e.CreateDay)
                .ToList();

            // Trả về báo cáo vắng học mới nhất
            return absentReport;
        }


        public List<Absentreport> AbsentReport_ShowAll()
        {

            // truy vấn
            var absentReport = _context.Absentreports.Include(r => r.Student).Include(r => r.Parent).ToList();

            // kiểm tra xem cso bản báo cáo nào trả về k hông
            if (absentReport != null)
            {
                return absentReport;
            }
            else
            {
                return null;
            }
        }

        public List<Absentreport> AbsentReport_RangeTime(DateTime timeStart, DateTime timeEnd)
        {
            // Bắt đầu ngày truyền vào lúc 00:00
            timeStart = timeStart.Date;

            // Kết thúc ngày truyền vào lúc 23:59:59
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            // Truy vấn với Include
            var absentReports = _context.Absentreports
                .Include(r => r.Student)  // Nạp thuộc tính Student
                .Include(r => r.Parent)   // Nạp thuộc tính Parent
                .Where(r => r.CreateDay >= timeStart && r.CreateDay <= timeEnd)
                .ToList(); // Trả về danh sách đối tượng ẩn danh

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return absentReports;
        }


        //================================================Alert==================================
        public List<Alert> Alert_ID(int alertId)
        {
            // Lấy thông báo cảnh báo theo ID và sắp xếp theo thời gian cảnh báo giảm dần
            var alert = _context.Alerts
                .Include(a => a.Student)  // Nạp đối tượng Student
                .Where(a => a.AlertId == alertId)
                .OrderByDescending(a => a.AlertTime)
                .ToList();


            // Trả về thông báo cảnh báo
            return alert;
        }

        public List<Alert> Alert_ShowAll()
        {
            // Truy vấn tất cả các thông báo cảnh báo với các thuộc tính liên quan
            var alerts = _context.Alerts
                .Include(a => a.Student)  // Nạp đối tượng Student
                .ToList();

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return alerts;
        }
        public List<Alert> Alert_RangeTime(DateTime timeStart, DateTime timeEnd)
        {
            // Bắt đầu ngày truyền vào lúc 00:00
            timeStart = timeStart.Date;

            // Kết thúc ngày truyền vào lúc 23:59:59
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            // Truy vấn với Include
            var alerts = _context.Alerts
                .Include(a => a.Student)  // Nạp thuộc tính Student
                .Where(a => a.AlertTime >= timeStart && a.AlertTime <= timeEnd)
                .ToList(); // Trả về danh sách các thông báo cảnh báo

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return alerts;
        }

        //==================================EntryLog=========================================================

        public List<Entrylog> Entrylog_ID(int studentId)
        {
            // Lấy tất cả bản ghi điểm danh theo ID học sinh
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student)  // Nạp đối tượng Student
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.LogTime)
                .ToList(); // Trả về danh sách các bản ghi điểm danh

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return entrylogs;
        }

        public List<Entrylog> Entrylog_ShowAll()
        {
            // Truy vấn tất cả các bản ghi điểm danh với các thuộc tính liên quan
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student)  // Nạp đối tượng Student
                .ToList();

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return entrylogs;
        }

        public List<Entrylog> Entrylog_RangeTime(DateTime timeStart, DateTime timeEnd)
        {
            // Bắt đầu ngày truyền vào lúc 00:00
            timeStart = timeStart.Date;

            // Kết thúc ngày truyền vào lúc 23:59:59
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            // Truy vấn với Include
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student)  // Nạp thuộc tính Student
                .Where(e => e.LogTime >= timeStart && e.LogTime <= timeEnd)
                .ToList(); // Trả về danh sách các bản ghi điểm danh

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return entrylogs;
        }

        //==================================studentInformation=========================================================

        public List<Student> StudentInfor_ID(int studentId)
        {
            // Lấy thông tin học sinh theo ID
            var student = _context.Students
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Parent)
                .ToList();

            // Trả về học sinh, có thể là null nếu không tồn tại
            return student;
        }

        public List<Student> StudentInfor_ShowAll()
        {
            // Truy vấn tất cả các học sinh với các thuộc tính liên quan
            var students = _context.Students
                .Include(s => s.Parent)
                .ToList();

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return students;
        }

        public List<Student> StudentInfor_RangeTime(DateTime timeStart, DateTime timeEnd)
        {
            // Bắt đầu ngày truyền vào lúc 00:00
            timeStart = timeStart.Date;

            // Kết thúc ngày truyền vào lúc 23:59:59
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            // Lấy các học sinh có bản ghi điểm danh trong khoảng thời gian
            var students = _context.Students
                .Where(s => s.Entrylogs.Any(e => e.LogTime >= timeStart && e.LogTime <= timeEnd))
                .Include(s => s.Parent)
                .ToList();

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return students;
        }

        //==================================Emtrylater=========================================================
        public List<Entrylog> Entrylater_ID(int studentId)
        {
            // Query the database
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student) // Ensure Student is loaded
                .Where(e =>
                    (e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                    (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime))
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.LogTime)
                .ToList();

            return entrylogs;
        }

        public List<Entrylog> Entrylater_Today()
        {

            // Query the database
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student) // Ensure Student is loaded
                .Where(e =>
                    (e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                    (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime))
                .Where(e => e.LogTime.Date == today) // Ensure only today's date is considered
                .OrderByDescending(e => e.LogTime)
                .ToList();

            return entrylogs;
        }

        public List<Entrylog> EntryLater_RangeTime()
        {
            // Query the database
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student) // Ensure Student is loaded
                .Where(e =>
                    (e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                    (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime))
                .OrderByDescending(e => e.LogTime)
                .ToList();

            return entrylogs;
        }

        //======================================================Add , edit , delete=======================

        public bool AddStudent(Parent parent, Student student)
        {
            try
            {
                // kiểm tra có học sinh và phu huynh truyền vào không
                if (parent == null || student == null)
                {
                    AnsiConsole.MarkupLine("[red]không có thông tin của học và phụ huynh[/]");
                    return false;
                }

                // Add parent to the context
                _context.Parents.Add(parent);

                // Save changes and retrieve ParentId
                _context.SaveChanges();
                student.ParentId = parent.ParentId; // thiết lập 

                // Add student to the context
                _context.Students.Add(student);

                // Save changes for student
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                AnsiConsole.MarkupLine($"[red]Lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();

                return false;
            }
        }


        public bool DeleteStudent(int id)
        {
            try
            {
                // tìm học sinh có id 
                var student = _context.Students.Find(id);
                if (student == null)
                {
                    AnsiConsole.MarkupLine("[red]Không tìm thấy học sinh![/]");
                    return false;
                }

                //Tìm phụ huynh của học sinh
                var parent = _context.Parents.Find(student.ParentId);

                // xóa phụ huynh
                if (parent != null)
                {
                    _context.Parents.Remove(parent);
                }

                // xóa học ssinh
                _context.Students.Remove(student);


                // lưu vào database
                _context.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {

                AnsiConsole.MarkupLine($"[red]Lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();

                return false;
            }
        }

        public bool UpdateStudent(int studentId)
        {
            try
            {
                // tìm học sinh
                var student = _context.Students.Find(studentId); // Tìm học sinh trong cơ sở dữ liệu bằng ID
                if (student != null)
                {
                    var parent = _context.Parents.Find(student.ParentId); // Tìm phụ huynh tương ứng
                    if (parent != null)
                    {
                        // Gọi phương thức cập nhật thông tin phụ huynh
                        _context.Parents.Update(view.UpdateParent(parent));
                        _context.SaveChanges();
                    }

                    _context.Students.Update(view.UpdateStudentInfo(student)); // Gọi phương thức cập nhật thông tin học sinh

                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu


                    return true; // Indicate success
                }
                else
                {
                    AnsiConsole.Markup("[red]Không tìm thấy hoc sinh.[/]");
                    AnsiConsole.WriteLine();
                    return false;
                }
            }
            catch (Exception ex)
            {

                AnsiConsole.MarkupLine($"[red]lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();

                return false;
            }
        }


        // ===============================AjustTime================================
        public async void AdjustScheduler(int hour1, int minutes1, int hour2, int minutes2)
        {
            // Dừng scheduler hiện tại
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Shutdown();

            // Khởi động lại scheduler với thời gian mới
            await schedulerService.StartScheduler(hour1, minutes1, hour2, minutes2);
        }


        //==========================================Parent=========================
        public List<Entrylog> Entrylog_Parent_RangeTime(int? studentId, DateTime timeStart, DateTime timeEnd)
        {
            // Bắt đầu ngày truyền vào lúc 00:00
            timeStart = timeStart.Date;

            // Kết thúc ngày truyền vào lúc 23:59:59
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            // Lấy tất cả bản ghi điểm danh theo ID học sinh
            var entrylogs = _context.Entrylogs
                .Include(e => e.Student)  // Nạp đối tượng Student
                .Where(e => (e.StudentId == studentId) && (e.LogTime >= timeStart && e.LogTime <= timeEnd))
                .OrderByDescending(e => e.LogTime)
                .ToList(); // Trả về danh sách các bản ghi điểm danh

            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return entrylogs;
        }

        public List<Entrylog> Entrylog_Parent_ShowAll(int? studentId)
        {
            // Truy vấn tất cả các bản ghi điểm danh với các thuộc tính liên quan
            var entrylogs = _context.Entrylogs
                 .Include(e => e.Student)  // Nạp đối tượng Student
                 .Where(e => e.StudentId == studentId)
                 .OrderByDescending(e => e.LogTime)
                 .ToList(); // Trả về danh sách các bản ghi điểm danh


            // Trả về danh sách kết quả, có thể là rỗng nhưng không phải null
            return entrylogs;
        }


        public bool Send_Report(int? id, string reason)
        {
            try
            {
                var student = _context.Students
                                    .Where(e => e.StudentId == id)
                                    .Select(e => new { e.StudentId, e.Parent.ParentId })
                                    .FirstOrDefault();

                if (student != null)
                {


                    // Tạo đối tượng báo cáo vắng học và lưu vào DbContext
                    Absentreport report = new Absentreport()
                    {
                        ParentId = student.ParentId,
                        CreateDay = DateTime.Now,
                        StudentId = student.StudentId,
                        Reason = reason
                    };

                    _context.Absentreports.Add(report); // Thêm báo cáo vào DbContext
                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    return true;

                }
                else
                {
                    AnsiConsole.Markup("[red]Không có học sinh trong hệ thống.[/]");
                    AnsiConsole.WriteLine();
                    return false;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]lỗi: {ex.Message}[/]");
                AnsiConsole.WriteLine();

                return false;
            }
        }


        public List<Absentreport> Show_Report(int? id)
        {
           
                // Lấy danh sách báo cáo vắng học của học sinh
                var multiReport = _context.Absentreports
                                         .Where(e => e.StudentId == id)
                                         .Include(e => e.Parent)
                                         .Include(e => e.Student)
                                         .OrderByDescending(e => e.CreateDay)
                                         .ToList();

               return multiReport;
        }


    }
}

