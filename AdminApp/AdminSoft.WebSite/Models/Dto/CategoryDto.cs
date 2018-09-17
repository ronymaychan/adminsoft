using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Models.Dto
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}