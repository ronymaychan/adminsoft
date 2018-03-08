/*
 *  master
 *      developer
 *          feature_cat_emp 1.0.0
 */
  
using AdminSoft.Data.Interfaces.Employees;
using AdminSoft.Domain.Employees;
using AdminSoft.WebSite.Helpers;
using AdminSoft.WebSite.Models;
using PLNFramework.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminSoft.WebSite.Controllers
{
    [AuthorizeResource(ActionKey = "ACCESS", ResourceKey = "EMPLOYEE")]
    public class EmployeeController : Controller
    {
        IEmployeeRepository repository;
        FileUploadHelper fileUpload;
        public EmployeeController(IEmployeeRepository repository, FileUploadHelper fileUpload)
        {
            this.repository = repository;
            this.fileUpload = fileUpload;
        }
        [AuthorizeResource(ActionKey = "ACCESS", ResourceKey = "EMPLOYEE")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEmployees(int offset, int limit, string search, string sort = "FirstName", string order="ASC")
        {
            int rows = 0;
            IEnumerable<Employee> employees = repository.QueryPage(search, out rows, (offset / limit), limit, sort + " " + order).ToList();
            var models = MapperHelper.Map<IEnumerable<EmployeeViewModels>>(employees);
            
            var model = new
            {
                total = rows,
                rows = models,
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeResource(ActionKey = "READ", ResourceKey = "EMPLOYEE")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var employee = repository.Find(id);

            if (employee == null)
            {
                this.BoostrapAlertWarning("No encontro el empleado que desea editar");
                return RedirectToAction("Index");
            }

            var model = MapperHelper.Map<EmployeeViewModels>(employee);
            if (employee.UrlAvatar != null)
                model.UrlAvatar = Url.Content(FileUploadHelper.folder + "/" + employee.UrlAvatar);

            return View(model);
        }

        [AuthorizeResource(ActionKey = "WRITE", ResourceKey = "EMPLOYEE")]
        public ActionResult Create() {
            return View(new EmployeeViewModels());
        }

        [AuthorizeResource(ActionKey = "WRITE", ResourceKey = "EMPLOYEE")]
        [HttpPost]
        public ActionResult Create(EmployeeViewModels model)
        {
            if (ModelState.IsValid)
            {
                var employee = MapperHelper.Map<Employee>(model);
                repository.Insert(employee);

                var name = fileUpload.SaveImageFile(model.Avatar);
                if (name != null)
                {
                    employee.UrlAvatar = name;
                    repository.Update(employee);
                }

                this.BoostrapAlertSuccess("Ha agregado un nuevo empleado");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [AuthorizeResource(ActionKey = "MODIFY", ResourceKey = "EMPLOYEE")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var employee = repository.Find(id);

            if (employee == null)
            {
                this.BoostrapAlertWarning("No encontro el empleado que desea editar");
                return RedirectToAction("Index");
            }

            var model = MapperHelper.Map<EmployeeViewModels>(employee);

            if (employee.UrlAvatar != null)
                model.UrlAvatar = Url.Content(FileUploadHelper.folder + "/" + employee.UrlAvatar);

            return View(model);
        }

        [AuthorizeResource(ActionKey = "MODIFY", ResourceKey = "EMPLOYEE")]
        [HttpPost]
        public ActionResult Edit(EmployeeViewModels model)
        {
            if (ModelState.IsValid) {
                var employee = repository.Find(model.EmployeeId);

                if (employee == null)
                {
                    this.BoostrapAlertWarning("No encontro el empleado que desea editar");
                    return RedirectToAction("Index");
                }

                MapperHelper.Map(model, employee);
                repository.Update(employee);

                if (model.Avatar != null) {
                    var imageName = fileUpload.SaveImageFile(model.Avatar, employee.UrlAvatar);
                    if (imageName != null) {
                        employee.UrlAvatar = imageName;
                        repository.Update(employee);
                    }
                }

                this.BoostrapAlertSuccess(string.Format("Ha actualizado al empleado {0} {1}", employee.FirstName, employee.LastName));
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [AuthorizeResource(ActionKey = "DELETE", ResourceKey = "EMPLOYEE")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var employee = repository.Find(id);

            if (employee == null)
            {
                this.BoostrapAlertWarning("No encontro el empleado que desea eliminar");
                return RedirectToAction("Index");
            }

            var model = MapperHelper.Map<EmployeeViewModels>(employee);

            if (employee.UrlAvatar != null)
                model.UrlAvatar = Url.Content(FileUploadHelper.folder + "/" + employee.UrlAvatar);

            return View(model);
        }

        [AuthorizeResource(ActionKey = "DELETE", ResourceKey = "EMPLOYEE")]
        [HttpPost]
        public ActionResult Delete(EmployeeViewModels model)
        {
            var employee = repository.Find(model.EmployeeId);
            if (employee == null)
            {
                this.BoostrapAlertWarning("No encontro el empleado que desea eliminar");
                return RedirectToAction("Index");
            }

            repository.Delete(employee);

            if (employee.UrlAvatar != null)
                fileUpload.DeleteImagenFile(employee.UrlAvatar);

            this.BoostrapAlertSuccess(string.Format("Ha elimiando al empleado {0} {1}", employee.FirstName, employee.LastName));
            return RedirectToAction("Index");
        }
    }
}