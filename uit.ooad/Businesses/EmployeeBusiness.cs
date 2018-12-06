﻿using System.Collections.Generic;
using System.Threading.Tasks;
using uit.ooad.DataAccesses;
using uit.ooad.Models;

namespace uit.ooad.Businesses
{
    public class EmployeeBusiness
    {
        public static Task<Employee> Add(Employee employee) => EmployeeDataAccess.Add(employee);
        public static Employee Get(string employeeId) => EmployeeDataAccess.Get(employeeId);
        public static IEnumerable<Employee> Get() => EmployeeDataAccess.Get();
    }
}
