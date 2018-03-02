using AdminSoft.Data.Interfaces.Base;
using AdminSoft.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminSoft.Data.Interfaces.Employees
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        ICollection<Employee> QueryByFirstNameOrLastName(string firstName, string lastName);
        ICollection<Employee> QueryPage(string search, out int totalRows, int page = 0, int pageSize = 10, string orders = "");
    }
}
