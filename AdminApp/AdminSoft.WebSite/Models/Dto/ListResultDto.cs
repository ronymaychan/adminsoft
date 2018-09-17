using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Models.Dto
{
    public class ListResultDto<T>
    {
        public ListResultDto(){
            Items = new List<T>();
        }

        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}