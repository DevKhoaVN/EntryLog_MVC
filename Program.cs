using MySql.Data.MySqlClient;

using System.Text;
using System.Threading;
using EntryManagement;
using Microsoft.EntityFrameworkCore.Design;
using Spectre.Console;
using MySql.EntityFrameworkCore;
using TestMySql.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestMySql;
using Microsoft.Identity.Client;
using EntryManagement.Menu;
using EntryManagement.Service;
using Microsoft.Win32;
using System.Globalization;
namespace EntryManagement
{

    public class Program
    {


        public static async Task Main(string[] args)
        {
              
            // Hiển thị tiếng việt
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            AnsiConsole.Write(new FigletText("EntryLogManagement").Color(Color.Aquamarine1_1).Centered());

            //Khởi tạo Controller
            Controller con = new Controller();
          

            bool isCheckAll = true;
            while (isCheckAll)
            {

                switch (MainMenu.IndexMenu())
                {
                    // xử lí đăng kí tài khoản
                    case 1:
                        // khởi tạo và gọi phương thức đăng kí
                        con.Resigter();                       
                        break;

                    // Xử lí đăng nhập
                    case 2:


                        // xác định vai trò của user
                        int role = 0;

                        // xác định ID học sinh của của user
                        int? StudentID = 0;

                        if (con.Login(out role, out StudentID))
                        {
                            switch (role)
                            {
                                //Thao tác của admin
                                case 1:

                                    int hour1 = 12, miute1 = 00, hour2 = 22, miute2 = 3;
                                    SchedulerService service = new SchedulerService();
                                    await service.StartScheduler(hour1, miute1, hour2, miute2);

                                    bool isCheckAdmin = true;

                                    while (isCheckAdmin)
                                    {
                                        // Menucủa admin
                                        switch (MenuAdmin.AdminMenu())
                                        {
                                            // Thực hiện 1.Quản lí học sinh
                                            case 1:
                                                bool isCheckAdmin_1 = true;
                                                while (isCheckAdmin_1)
                                                {
                                                    switch (MenuAdmin.AdminStudentManagement1())
                                                    {
                                                        //  1.Quản lí học sinh -> 1.Thêm xửa xóa
                                                        case 1:

                                                            bool isCheckAdmin_1_1 = true;
                                                            while (isCheckAdmin_1_1)
                                                            {
                                                                switch (MenuAdmin.AdminStudentManagement1_1())
                                                                {
                                                                    // Thêm
                                                                    case 1:
                                                                        con.AddStudent();
                                                                        break;

                                                                    // Xóa
                                                                    case 2:
                                                                        con.DeleteStudent();
                                                                        break;

                                                                    // chỉnh
                                                                    case 3:
                                                                        con.UpdateStudent();
                                                                        break;

                                                                    case 0:

                                                                        // Quay lại trang trước đó
                                                                        isCheckAdmin_1_1 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        // 1.Quản lí học sinh-> 2.xem báo cáo vắng học
                                                        case 2:

                                                            bool isCheckAdmin_1_2 = true;
                                                            while (isCheckAdmin_1_2)
                                                            {
                                                                switch (MenuAdmin.AdminStudentManagement1_2())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:
                                                                        con.AbsentReport_SearchByID();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:
                                                                        con.AbsentReport_RangeTime();
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        con.AbsentReport_ShowAll();
                                                                        break;

                                                                    // Quay lại trang trước đó
                                                                    case 0:

                                                                        isCheckAdmin_1_2 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        // 1.Quản lí học sinh -> 3.Xem cảnh báo
                                                        case 3:

                                                            bool isCheckAdmin_1_3 = true;
                                                            while (isCheckAdmin_1_3)
                                                            {
                                                                switch (MenuAdmin.AdminStudentManagement1_3())
                                                                {
                                                                    // lọc theo id học sinh
                                                                    case 1:

                                                                        con.Alert_SearchByID();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        con.Alert_RangeTime();
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        con.Alert_ShowAll();
                                                                        break;

                                                                    // Quay lại trang trước đó
                                                                    case 0:

                                                                        isCheckAdmin_1_3 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        //1.Quản lí học sinh -> 4.Xem thông tin học sinh
                                                        case 4:
                                                            
                                                            bool isCheckAdmin_1_4 = true;
                                                            while (isCheckAdmin_1_4)
                                                            {
                                                                switch (MenuAdmin.AdminStudentManagement1_4())
                                                                {
                                                                    // Lọc theo id học sinh
                                                                    case 1:

                                                                        con.StudentInfor_SearchByID();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        con.StudentInfor_RangeTime();
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        con.StudentInfor_ShowAll();
                                                                        break;

                                                                    // Quay lại trang trước đó
                                                                    case 0:

                                                                        isCheckAdmin_1_4 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        case 0:

                                                            isCheckAdmin_1 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }

                                                break;

                                            // Thực hiện 2.Quản lí ra vào --------------------------------
                                            case 2:

                                                bool isCheckEntry = true;
                                                while (isCheckEntry)
                                                {
                                                    switch (MenuAdmin.AdminEntryLogManagement2())
                                                    {
                                                        // 2.Quản lí ra vào -> 1.Xem bảng học sinh ra vào
                                                        case 1:
                                                            
                                                            bool isCheckEntry_1 = true;
                                                            while (isCheckEntry_1)
                                                            {
                                                                switch (MenuAdmin.AdminEntryLogManagement2_1())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:

                                                                        con.Entrylog_SearchByID();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        con.Entrylog_RangeTime();
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:

                                                                        con.Entrylog_ShowAll();
                                                                        break;

                                                                    // Quay lại trang trước đó
                                                                    case 0:

                                                                        isCheckEntry_1 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }

                                                            break;

                                                        // 2.Quản lí ra vào -> 2.Xem báo cáo đi muộn
                                                        case 2:

                                                            bool isCheckEntry_2 = true;
                                                            while (isCheckEntry_2)
                                                            {
                                                                switch (MenuAdmin.AdminEntryLogManagement2_2())
                                                                {
                                                                    // Học theo đi muộn trong ngày
                                                                    case 1:

                                                                        con.Entrylater_Today();
                                                                        break;

                                                                    // Lọc theo ID học sinh
                                                                    case 2:

                                                                        con.Entrylater_SearchByID();
                                                                        break;

                                                                    // Hiển thị theo thời gian
                                                                    case 3:

                                                                        con.Entrylater_RangeTime();
                                                                        break;

                                                                    // Quay lại trang trước đó
                                                                    case 0:

                                                                        isCheckEntry_2 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }

                                                            break;

                                                        //2.Quản lí ra vào -> 3.Điều chỉnh thời gian cảnh báo 
                                                        case 3:

                                                            con.AjustTimeScheduler();
                                                            break;

                                                        // Quay lại trang trước đó
                                                        case 0:
                                                            isCheckEntry = false;
                                                            break;


                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;

                                                    }

                                                }
                                                break;

                                            // Thực hiện 3.Thực hiện kiểm tra ra vào
                                            case 3:

                                                Camera camera = new Camera();
                                                camera.TurnOn();
                                                break;

                                            case 0:

                                                return;
                                        }

                                    }
                                    break;

                                // Thao tác của parent
                                case 2:

                                    bool isCheckParent = true;
                                    while (isCheckParent)
                                    {
                                        switch (MenuParent.ParentMenu())
                                        {
                                            // 1. Xem thông tin ra vào
                                            case 1:


                                                bool isCheckParent1 = true;
                                                while (isCheckParent1)
                                                {
                                                    switch (MenuParent.ParentEntryLog())
                                                    {
                                                        // Lọc theo thời gian
                                                        case 1:

                                                            con.Entrylog_Parent_RangeTime(StudentID);
                                                            break;

                                                        // Hiển thị tất cả
                                                        case 2:
                                                            con.Entrylog_Parent_ShowAll(StudentID);
                                                            break;

                                                        case 0:

                                                            isCheckParent1 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }
                                                break;

                                            // 2. Xem báo cáo vắng học
                                            case 2:

                                              

                                                bool isCheckParent2 = true;
                                                while (isCheckParent2)
                                                {
                                                    switch (MenuParent.ParentAbsentReport())
                                                    {
                                                        // Gửi báo cáo vắng học
                                                        case 1:

                                                            con.SendReport(StudentID);
                                                            break;

                                                        // Hiển thị tất cả
                                                        case 2:
                                                            con.ShowAll_Report(StudentID);
                                                            break;

                                                        case 0:

                                                            isCheckParent2 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }
                                                break;

                                            // Hiển thị tất cả


                                            case 0:

                                                return;


                                            default:
                                                Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                break;
                                        }
                                    }

                                    break;
                            }

                        }

                        break;

                    case 0:

                        isCheckAll = false;
                        break;

                    default:

                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                        break;

                }
            }


        }
    }
}

// 
// "Data Source=DESKTOP-Q51CKKR\\SQLEXPRESS01;Initial Catalog=EntryLogManagement;Integrated Security=True;Trust Server Certificate=True"


// dotnet ef dbcontext scaffold -o Models -f -d "Data Source==DESKTOP-Q51CKKR\SQLEXPRESS01;Initial Catalog=EntryLogManagement;Integrated Security=True;Trust Server Certificate=True" "Microsoft.EntityFrameworkCore.SqlServer"


//= string connectionString = "Server=localhost;Database=entrylogmanagement;User ID=root;Password=Vakhoa205!";
//dotnet ef dbcontext scaffold "Server=localhost;Database=entrylogmanagement;User ID=root;Password=Vakhoa205!" MySql.EntityFrameworkCore -o Models -fcd

// string connectionString = "Server=localhost;Database=`entrylogmanagement`;User ID=root;Password=Vakhoa205!";
// dotnet ef dbcontext scaffold "Server=localhost;Database=entrylogmanagement;User ID=root;Password=Vakhoa205!" MySql.EntityFrameworkCore -o Models -f