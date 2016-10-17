using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Crushlush.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Authentication
            config.Filters.Add(new AuthorizeAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1.0/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
