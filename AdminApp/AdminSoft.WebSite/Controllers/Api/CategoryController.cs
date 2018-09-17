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
    public class CategoryController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(ListResultDto<CategoryDto>))]
        public IHttpActionResult Get(string term = null, int offset = 0, int limit = 20)
        {
            ListResultDto<CategoryDto> dtos = new ListResultDto<CategoryDto>();

            dtos.Items.Add(new CategoryDto() { Id = 1, Name = "MX" });
            dtos.Items.Add(new CategoryDto() { Id = 2, Name = "EUA" });

            return Ok(dtos);
        }

       
        [HttpGet]
        [Route("/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult Get(int id)
        {
            var dto = new CategoryDto() { Id = 1, Name = "MX" };
            return Ok(dto);
        }

        [HttpPost]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult Post(CategoryDto dto)
        {
            dto = new CategoryDto() { Id = 1, Name = "MX" };
            return Ok(dto);
        }

        [HttpPut]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult Put(CategoryDto dto)
        {
            dto = new CategoryDto() { Id = 1, Name = "MX" };
            return Ok(dto);
        }

        [HttpDelete]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult Delete(int id)
        {

            var dto = new CategoryDto() { Id = 1, Name = "MX" };
            return Ok(dto);
        }
    }
}
