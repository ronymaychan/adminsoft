using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AdminSoft.WebSite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(cors);

            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));


            // Rutas de API web
            config.MapHttpAttributeRoutes();

            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            config.Routes.MapHttpRoute(
               name: "ActionWithApi",
               routeTemplate: "api/{controller}/{Action}/{id}",
               defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
           );
        }
    }
}
