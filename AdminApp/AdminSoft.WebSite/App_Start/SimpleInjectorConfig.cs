using AdminSoft.Data;
using AdminSoft.Data.Employees;
using AdminSoft.Data.Interfaces.Base;
using AdminSoft.Data.Interfaces.Employees;
using AdminSoft.WebSite.Helpers;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AdminSoft.WebSite
{
    public class SimpleInjectorConfig
    {
        public static void RegisterInjectors() {

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.RegisterMvcIntegratedFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            //DataContext
            container.Register<IDataContext, AdminSoftContext>(Lifestyle.Scoped);

            //Repositorios
            container.Register<IEmployeeRepository, EmployeeRepository>();


            //Helpers
            container.Register<FileUploadHelper>();
        }
    }
}