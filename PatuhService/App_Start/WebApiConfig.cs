using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PatuhService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/{controller}/{userName}/{password} ",
                defaults: new { userId = RouteParameter.Optional, password = RouteParameter.Optional }
            );
        }
    }
}
