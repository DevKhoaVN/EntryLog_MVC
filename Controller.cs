using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace TestMySql
{
    public class Controller
    {
        private readonly ViewApplication _view;
        private readonly Service _service;
        public Controller()
        {
            _view = new ViewApplication();
            _service = new Service();
        }

        public bool Login(out int User_role, out int? Student_id)
        {
            User_role = 0;
            Student_id = 0;

            AnsiConsole.MarkupLine("[[[#DCA47C]Đăng nhập[/]]]");
            AnsiConsole.WriteLine();


            // Tào khoản đầu vào
            var userName = _view.GetStringPrompt("Nhập [green]tài khoản: [/]");


            // Mật khẩu đầu vào
            var password = _view.GetPasswordPrompt("Nhập [green]mật khẩu: [/]");

            AnsiConsole.WriteLine();

            var isCheckUser = _service.Validate_User(userName, password, out User_role, out Student_id);

            if (isCheckUser)
            {
                _view.TextReuslt("[#00ff00]Bạn đã đăng nhập thành công![/]");
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Resigter()
        {
            AnsiConsole.MarkupLine("[[[#DCA47C]Đăng kí[/]]]");
            AnsiConsole.WriteLine();

            var userName = _view.GetStringPrompt("Nhập [green]tài khoản: [/]");
            var password = _view.GetPasswordPrompt("Nhập [green]mật khẩu: [/]");
            var student_id = _view.GetStringPrompt("Nhập [green]ID học sinh: [/]");

            var isCheckRegister = _service.HandleRegister(userName, password, student_id);

            if (isCheckRegister)
            {
                _view.TextReuslt("[#00ff00]Bạn đã đăng kí thành công![/]");
            } else
            {
                _view.TextReuslt("[#red]Vui lòng đăng kí lại![/]");
            }
        }

        public void AbsentReport_SearchByID()
        {
            var id = _view.GetIntPrompt("Nhập [green]ID học sinh: [/]");

            var result = _service.AbsentReport_ID(id);
            if (result.Count > 0)
            {
                _view.AbsentReport_ID(result, id);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }
        public void AbsentReport_ShowAll()
        {

            var result = _service.AbsentReport_ShowAll();
            if (result.Count > 0)
            {

                _view.AbsentReport_ShowAll(result);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        public void AbsentReport_RangeTime()
        {
            var timeStart = _view.GetDate("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
            var timeEnd = _view.GetDate("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");

            var result = _service.AbsentReport_RangeTime(timeStart, timeEnd);

            if (result.Count > 0)
            {
                _view.AbsentReport_RangeTime(result, timeStart, timeEnd);
            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }



        //==============================================Alert==================================

        public void Alert_SearchByID()
        {
            var id = _view.GetIntPrompt("Nhập [green]ID học sinh: [/]");

            var result = _service.Alert_ID(id);
            if (result.Count > 0)
            {
                _view.Alert_ID(result, id);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }
        public void Alert_ShowAll()
        {

            var result = _service.Alert_ShowAll();
            if (result.Count > 0)
            {

                _view.Alert_ShowAll(result);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        public void Alert_RangeTime()
        {
            var timeStart = _view.GetDate("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
            var timeEnd = _view.GetDate("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");

            var result = _service.Alert_RangeTime(timeStart, timeEnd);

            if (result.Count > 0)
            {
                _view.Alert_RangeTime(result, timeStart, timeEnd);
            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        //==================================EntryLog=========================================================

        public void Entrylog_SearchByID()
        {
            var id = _view.GetIntPrompt("Nhập [green]ID học sinh: [/]");

            var result = _service.Entrylog_ID(id);
            if (result.Count > 0)
            {
                _view.Entrylog_ID(result, id);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }
        public void Entrylog_ShowAll()
        {

            var result = _service.Entrylog_ShowAll();
            if (result.Count > 0)
            {

                _view.Entrylog_ShowAll(result);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        public void Entrylog_RangeTime()
        {
            var timeStart = _view.GetDate("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
            var timeEnd = _view.GetDate("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");

            var result = _service.Entrylog_RangeTime(timeStart, timeEnd);

            if (result.Count > 0)
            {
                _view.Entrylog_RangeTime(result, timeStart, timeEnd);
            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        //==================================studentInformation=========================================================
        public void StudentInfor_SearchByID()
        {
            try
            {
                var id = _view.GetIntPrompt("Nhập [green]ID học sinh: [/]");

                var result = _service.StudentInfor_ID(id);
                if (result.Count > 0)
                {
                    _view.StudentInfor_ID(result, id);
                }
                else
                {
                    _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }
        public void StudentInfor_ShowAll()
        {

            var result = _service.StudentInfor_ShowAll();
            if (result.Count > 0)
            {

                _view.StudentInfor_ShowAll(result);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        public void StudentInfor_RangeTime()
        {
            var timeStart = _view.GetDate("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
            var timeEnd = _view.GetDate("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");

            var result = _service.StudentInfor_RangeTime(timeStart, timeEnd);

            if (result.Count > 0)
            {
                _view.StudentInfor_RangeTime(result, timeStart, timeEnd);
            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        //==================================Entrylater=========================================================
        public void Entrylater_SearchByID()
        {
            try
            {
                var id = _view.GetIntPrompt("Nhập [green]ID học sinh: [/]");

                var result = _service.Entrylater_ID(id);
                if (result.Count > 0)
                {
                    _view.Entrylater_ID(result, id);
                }
                else
                {
                    _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }

        public void Entrylater_Today()
        {
            try
            {

                var result = _service.Entrylater_Today();
                if (result.Count > 0)
                {
                    _view.Entrylater_ShowAll(result);
                }
                else
                {
                    _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }

        public void Entrylater_RangeTime()
        {
            try
            {
                

                var result = _service.EntryLater_RangeTime();
                if (result.Count > 0)
                {
                    _view.Entrylater_Today(result);
                }
                else
                {
                    _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }


        //==================================StudentManage=========================================================

        public void AddStudent()
        {
            try
            {
                Student student = new Student();

                student.Name = _view.GetStringPrompt("Nhập [green]tên học sinh[/]: ");
                student.Gender = _view.GetStringPrompt("Nhập [green]giới tính học sinh[/]: ");
                student.DayOfBirth = _view.GetDate("Nhập ngày sinh học sinh (yyyy/MM/dd): ");
                student.Class = _view.GetStringPrompt("Nhập[green] lớp học sinh[/]: ");
                student.Address = _view.GetStringPrompt("Nhập [green]địa chỉ học sinh[/]: ");
                student.Phone = _view.GetValidPhoneNumber("Nhập [green] số điện thoại của học sinh[/]: ");

                Parent parent = new Parent();
                parent.Name = _view.GetStringPrompt("Nhập [green]tên phụ huynh[/]: ");
                parent.Phone = _view.GetValidPhoneNumber("Nhập [green]số điện thoại phụ huynh[/]: ");
                parent.Email = _view.GetValidEmail("Nhập[green] Email phụ huynh:[/] ");
                parent.Address = _view.GetStringPrompt("Nhập [green]địa chỉ phụ huynh[/]: ");




                var result = _service.AddStudent(parent, student);
                if (result)
                {
                    _view.TextReuslt("[#00ff00]Thêm học sinh thành công![/]");
                }
                else
                {
                    _view.TextReuslt("[red]Thêm học sinh thất bại![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }

        public void DeleteStudent()
        {
            try
            {
                var id = _view.GetIntPrompt("Nhập[green] ID học sinh bạn muốn xóa :[/]");

                var isCheckStudent = _service.DeleteStudent(id);

                if (isCheckStudent)
                {
                    _view.TextReuslt("[#00ff00]Xóa học sinh thành công![/]");
                }
                else
                {
                    _view.TextReuslt("[#00ff00]Xóa học sinh thất bại![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }

        }


        public void UpdateStudent()
        {
            try
            {
                var id = _view.GetIntPrompt("Nhập[green] ID học sinh bạn muốn chỉnh sửa :[/]");
               

                var isCheckStudent = _service.UpdateStudent(id);

                if (isCheckStudent)
                {
                    _view.TextReuslt("[#00ff00]Chỉnh sửa thông tin học sinh thành công![/]");
                }
                else
                {
                    _view.TextReuslt("[#00ff00]Chỉnh sửa thông tin học sinh thất bại![/]");
                }
            }
            catch (Exception ex)
            {
                _view.TextReuslt($"[red]Có lỗi xảy ra: {ex.Message}[/]");
            }
        }

        //========================================================AjustTimeScheduler=======================================

        public async void AjustTimeScheduler()
        {
            int hour1 = _view.GetValidHour("Nhập [green]giờ bạn muốn thay đổi (buổi sáng)[/]: ");
            int minutes1 = _view.GetValidMinute("Nhập [green]phút bạn muốn thay đổi (buổi sáng)[/]: ");

            int hour2 = _view.GetValidHour("Nhập [green]giờ bạn muốn thay đổi (buổi chiều)[/]: ");
            int minutes2 = _view.GetValidMinute("Nhập [green]phút bạn muốn thay đổi (buổi chiều)[/]: ");

            _service.AdjustScheduler(hour1, minutes1, hour2, minutes2);
        }


        //=======================================

        public void Entrylog_Parent_RangeTime(int? id)
        {
            var timeStart = _view.GetDate("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
            var timeEnd = _view.GetDate("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");


            var result = _service.Entrylog_Parent_RangeTime(id , timeStart , timeEnd);
            if (result.Count > 0)
            {
                _view.Entrylog_ID(result, id);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }
        public void Entrylog_Parent_ShowAll(int? id)
        {

            var result = _service.Entrylog_Parent_ShowAll(id);
            if (result.Count > 0)
            {

                _view.Entrylog_ShowAll(result);

            }
            else
            {
                _view.TextReuslt("[red]Không tìm thấy bản ghi nào![/]");
            }
        }

        //===============

        public void SendReport(int? id)
        {
            var reason = _view.GetStringPrompt("Nhập [green]lí do vắng học[/]:");

            var result = _service.Send_Report(id , reason);

            if(result)
            {
                _view.TextReuslt("[green]Bạn đã gửi thành công[/]");
            }else
            {
                _view.TextReuslt("[red]Bạn đã gửi thất bại[/]");
            }

        }

        public void ShowAll_Report(int? id)
        {
            var result = _service.Show_Report(id);
            if( result.Count > 0)
            {
                _view.AbsentReport_ShowAll(result);
            }
            else
            {
                _view.TextReuslt("[red]Không có bản ghi trong hệ thống[/]");
            }
        }

    }
}
