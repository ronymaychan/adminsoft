using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Models
{
    public class EmployeeViewModels
    {
        public EmployeeViewModels()
        {
        }
        [Display(AutoGenerateField = false)]
        public int? EmployeeId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [MaxLength(100)]
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [MaxLength(100)]
        [Display(Name = "Estado")]
        public string State { get; set; }
        [MaxLength(100)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }
        [Display(AutoGenerateField = false)]
        public string UrlAvatar { get; set; }
        [Display(Name = "Avatar", AutoGenerateField=false)]
        public HttpPostedFileBase Avatar { get; set; }
    }
}