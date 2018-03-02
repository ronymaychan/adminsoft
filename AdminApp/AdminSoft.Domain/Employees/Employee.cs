using System;
using System.ComponentModel.DataAnnotations;

namespace AdminSoft.Domain.Employees
{
    public class Employee
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string UrlAvatar { get; set; }
        public DateTime? LastUpdate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
