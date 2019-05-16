using BootstrapIntroduction.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BootstrapIntroduction.App_Start
{
    public class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {

            config.Filters.Add(new ValidationActionFilterAttribute());
            config.Filters.Add(new OnApiExceptionAttribute());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}