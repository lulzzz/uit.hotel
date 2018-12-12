﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uit.ooad.DataAccesses;
using uit.ooad.Models;

namespace uit.ooad.Businesses
{
    public class EmployeeBusiness
    {
        public static Task<Employee> Add(Employee employee)
        {
            var employeeInDatabase = Get(employee.Id);
            if (employeeInDatabase != null) return null;

            employee.Position = employee.Position.GetManaged();
            return EmployeeDataAccess.Add(employee);
        }
        public static void ChangePassword(string id, string password, string newPassword)
        {
            var employee = EmployeeBusiness.Get(id);
            if (employee == null) throw new Exception("Không tim thấy tên đăng nhập trong hệ thống");

            else if (employee.Password != password) throw new Exception("Mật khẩu không đúng");

            EmployeeDataAccess.ChangePassword(employee, newPassword);
        }

        public static void CheckLogin(string id, string password)
        {
            var employee = Get(id);
            if(employee != null)
            {
                if(employee.Password == password) return;
            } 
            throw new Exception("Tài khoản hoặc mật khẩu không chính xác");
        }
        public static Employee Get(string employeeId) => EmployeeDataAccess.Get(employeeId);

        public static IEnumerable<Employee> Get() => EmployeeDataAccess.Get();

        public static async void CreateAdminIfNotExists()
        {
            if (Get().Count() != 0) return;

            var position = await PositionBusiness.Add(new Position()
            {
                Id = 1,
                Name = "Quản trị viên",
                PermissionCreateBill = true,
                PermissionCreateBooking = true,
                PermissionCreateEmployee = true,
                PermissionCreateFloor = true,
                PermissionCreateHouseKeeping = true,
                PermissionCreatePatron = true,
                PermissionCreatePosition = true,
                PermissionCreateRate = true,
                PermissionCreateReceipt = true,
                PermissionCreateRoom = true,
                PermissionCreateRoomKind = true,
                PermissionCreateService = true,
                PermissionCreateServicesDetail = true,
                PermissionCreateVolatilityRate = true
            });

            await EmployeeBusiness.Add(new Employee()
            {
                Id = "admin",
                Name = "Quản trị viên",
                Password = "????",
                Address = "Unknown",
                PhoneNumber = "Unknown",
                Birthdate = DateTime.Now,
                StartingDate = DateTime.Now,
                Position = position,
            });
        }
    }
}
