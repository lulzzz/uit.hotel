﻿using System.Collections.Generic;
using System.Threading.Tasks;
using uit.hotel.Models;

namespace uit.hotel.DataAccesses
{
    public class EmployeeDataAccess : RealmDatabase
    {
        public static async Task<Employee> Add(Employee employee)
        {
            await Database.WriteAsync(realm =>
            {
                employee.IsActive = true;
                employee = realm.Add(employee);
            });
            return employee;
        }

        public static async Task<Employee> Update(Employee employeeInDatabase, Employee employee)
        {
            await Database.WriteAsync(realm =>
            {
                employeeInDatabase.Name = employee.Name;
                employeeInDatabase.IdentityCard = employee.IdentityCard;
                employeeInDatabase.PhoneNumber = employee.PhoneNumber;
                employeeInDatabase.Address = employee.Address;
                employeeInDatabase.Email = employee.Email;
                employeeInDatabase.Gender = employee.Gender;
                employeeInDatabase.Birthdate = employee.Birthdate;
                employeeInDatabase.StartingDate = employee.StartingDate;
                employeeInDatabase.Position = employee.Position;
            });
            return employeeInDatabase;
        }

        public static async void ChangePassword(Employee employee, string newPassword) =>
            await Database.WriteAsync(realm => employee.Password = newPassword);

        public static async void SetIsActiveAccount(Employee employee, bool isActive)
        {
            await Database.WriteAsync(realm => employee.IsActive = isActive);
        }

        public static Employee Get(string employeeId) => Database.Find<Employee>(employeeId);
        public static IEnumerable<Employee> Get() => Database.All<Employee>();
    }
}
