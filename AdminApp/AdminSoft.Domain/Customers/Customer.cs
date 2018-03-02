using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plenumsoft.Domain.Entities.Customers
{
    public class Customer
    {
        public int? CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        public int? CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class Product
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TypeProduct> Types { get; set; }
    }
    public class TypeProduct
    {
        public int? TypeId { get; set; }
        public string Name { get; set; }
    }
}
