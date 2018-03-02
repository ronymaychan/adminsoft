using AdminSoft.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AdminSoft.Data.Interfaces.Employees;
using AdminSoft.Data.Interfaces.Base;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using AdminSoft.Domain.Employees;

namespace AdminSoft.Data.Employees
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        // constructor
        public EmployeeRepository(IDataContext context) : base((DbContext)context)
        {
        }

        public ICollection<Employee> QueryByFirstNameOrLastName(string firstName, string lastName)
        {

            var query = _context.Employees.AsQueryable();


            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(e => e.FirstName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(e => e.LastName.Contains(lastName));


            return query.ToList();
        }
        public ICollection<Employee> QueryPage(string search, out int totalRows, int page = 0, int pageSize = 10, string orders = "")
        {

            Expression<Func<Employee, bool>> filter = x => true;
            int totalPages;

            if (!string.IsNullOrEmpty(search))
                filter = c => c.FirstName.Contains(search) || c.LastName.Contains(search) ||
                    c.City.Contains(search) || c.State.Contains(search);

            var items = QueryPage(filter, out totalPages, out totalRows, page, pageSize, orders).ToArray();

            return items;
        }
    }
}
