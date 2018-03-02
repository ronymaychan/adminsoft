using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSoft.WebSite.Helpers;

namespace AdminSoft.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var context = new PLNFramework.Security.AppSecurityContext())
            //{
            //    var repository = new PLNFramework.Security.Repositories.UserPermissionRepository(context);
            //    var data = repository.GetAll().ToList();
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}