using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using AdminSoft.Data.Employees;
using AdminSoft.Data;
using AdminSoft.Data.Interfaces.Base;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using AdminSoft.Data.Interfaces.Employees;
using AdminSoft.Domain.Employees;

namespace AdminSoft.Test
{
    [TestClass]
    public class EmployeeTest
    {
        [TestMethod]
        public void EmployeeFindByIdTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);
                var employee = service.Find(1);
                Assert.IsNotNull(employee);
            }
        }
        [TestMethod]
        public void EmployeeGetAllTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);
                var employees = service.GetAll();
                Assert.IsNotNull(employees);
            }
        }
        [TestMethod]
        public void EmployeeQueryTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);

                //Filters
                var employee = new Employee() { City = "London" };

                //exec query
                var employees = service.Query(employee, "City DESC");
                Assert.IsNotNull(employee);
            }
        }
        [TestMethod]
        public void EmployeeQueryPageTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);
                int totalRows = 0;

                //Filters
                var employee = new Employee() { City = "London" };

                //Consultar paginado
                var employees = service.QueryPage(null, out totalRows, 0, 10, "City ASC");
                Assert.IsNotNull(employees);
            }
        }
        [TestMethod]
        public void EmployeeInsertTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);
                Employee employee = new Employee()
                {
                    FirstName = "Petter",
                    LastName = "King",
                    Address = "908 W. Capital Way",
                    City = "London",
                    Phone = "(71) 555-4848"
                };
                service.Insert(employee);
                Assert.IsNotNull(employee.EmployeeId);
            }
        }
        [TestMethod]
        public void EmployeeUpdateTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);

                //Get employee to update
                Employee employee = new Employee() { FirstName = "Janet" };
                employee = service.Query(employee).FirstOrDefault();
                Assert.IsNotNull(employee);

                //update employee
                employee.Phone = "(71) 555-4848";
                service.Update(employee);
                Assert.IsNotNull(employee.EmployeeId);
            }
        }
        [TestMethod]
        public void EmployeeDeleteTest()
        {
            using (IDataContext context = new AdminSoftContext())
            {
                IEmployeeRepository service = new EmployeeRepository(context);
                int id = 0;
                //insert employee to delete
                Employee employee = new Employee()
                {
                    FirstName = "Petter",
                    LastName = "Leverling",
                    Address = "908 W. Capital Way",
                    City = "London",
                    Phone = "(71) 555-4848"
                };
                service.Insert(employee);
                id = employee.EmployeeId.Value;
                Assert.IsNotNull(employee);

                //delete employee
                employee = service.Find(id);
                service.Delete(employee);
                Assert.IsNotNull(employee.EmployeeId);
            }
        }
    }
}
