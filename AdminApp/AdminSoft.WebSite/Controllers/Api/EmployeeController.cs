using AdminSoft.Data.Interfaces.Employees;
using AdminSoft.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PLNFramework.Security.Filters;
using AdminSoft.WebSite.Models.Dto;
using AdminSoft.WebSite.Helpers;

namespace AdminSoft.WebSite.Controllers.Api
{
    [AuthorizeResourceApiAttribute(ActionKey = "ACCESS", ResourceKey = "EMPLOYEE")]
    public class EmployeeController : ApiController
    {
        IEmployeeRepository repository;
        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        [AuthorizeResourceApiAttribute(ActionKey = "READ", ResourceKey = "EMPLOYEE")]
        public IHttpActionResult Get(string term = null, int offset = 0, int limit = 20)
        {
            int totalRows = 0;
            var employee = repository.QueryPage(term, out totalRows, offset, limit, "EmployeeId");
            var dtos = MapperHelper.Map<List<Employeedto>>(employee);

            foreach (var item in dtos) {
                CalculateUrl(item);
            }

            var response = new
            {
                totalRow = totalRows,
                limit = limit,
                offset = offset,
                next = totalRows > (offset + limit),
                previous = (offset - limit) >= 0,
                items = dtos

            };
			
            return Ok(response);
        }

        [AuthorizeResourceApiAttribute(ActionKey = "READ", ResourceKey = "EMPLOYEE")]
        public IHttpActionResult Get(int id)
        {
            var employee = repository.Find(id);
            if (employee == null)
                return BadRequest("invalid employee");
            var dto = MapperHelper.Map<Employeedto>(employee);
            CalculateUrl(dto);
            return Ok(dto);
        }

        [AuthorizeResourceApiAttribute(ActionKey = "WRITE", ResourceKey = "EMPLOYEE")]
        public IHttpActionResult Post(Employeedto dto)
        {
            if (dto == null)
                return BadRequest("The employee is required");
            var employee = MapperHelper.Map<Employee>(dto);
            repository.Insert(employee);
            return Ok(MapperHelper.Map<Employeedto>(employee));
        }

        [AuthorizeResourceApiAttribute(ActionKey = "MODIFY", ResourceKey = "EMPLOYEE")]
        public IHttpActionResult Put(Employeedto dto)
        {
            if (dto == null)
                return BadRequest("The employee is required");

            if (dto.EmployeeId == null)
                return BadRequest("The employee identifier is required");

            var employee = MapperHelper.Map<Employee>(dto);
            employee.LastUpdate = DateTime.Now;
            repository.Update(employee);
            return Ok(MapperHelper.Map<Employeedto>(employee));
        }

        [AuthorizeResourceApiAttribute(ActionKey = "DELETE", ResourceKey = "EMPLOYEE")]
        public IHttpActionResult Delete(int id)
        {
            var employee = repository.Find(id);
            if (employee == null)
                return NotFound();
            repository.Delete(employee);
            return Ok(MapperHelper.Map<Employeedto>(employee));
        }

        private void CalculateUrl(Employeedto dto) {
            if (!string.IsNullOrEmpty(dto.UrlAvatar))
            {
                var nameImage = dto.UrlAvatar;
                dto.UrlAvatar = Url.Content(string.Format("{0}/{1}", FileUploadHelper.folder, nameImage));
                dto.UrlAvatarThumbnail = Url.Content(string.Format("{0}/{1}", FileUploadHelper.folderThumbnail, nameImage));
            }
            else {
                dto.UrlAvatar = null;
                dto.UrlAvatarThumbnail = null;
            }
        }
    }
}
