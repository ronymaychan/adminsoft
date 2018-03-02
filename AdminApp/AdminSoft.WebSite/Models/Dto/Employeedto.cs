using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Models.Dto
{
    public class Employeedto
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string UrlAvatar { get; set; }
        public string UrlAvatarThumbnail { get; set; }
    }
}