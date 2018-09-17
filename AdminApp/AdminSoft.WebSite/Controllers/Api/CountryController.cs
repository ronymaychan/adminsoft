using AdminSoft.WebSite.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AdminSoft.WebSite.Controllers.Api
{
    public class CountryController : ApiController
    {
        [HttpGet]
        [ActionName("GetAll")]
        [ResponseType(typeof(ListResultDto<CountryDto>))]
        public IHttpActionResult Get(string term = null, int offset = 0, int limit = 20)
        {
            ListResultDto<CountryDto> dtos = new ListResultDto<CountryDto>();

            dtos.Items.Add(new CountryDto() { Id = 1, Name = "MX" });
            dtos.Items.Add(new CountryDto() { Id = 2, Name = "EUA" });

            return Ok(dtos);
        }
    }
}
