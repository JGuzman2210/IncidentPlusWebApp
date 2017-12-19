using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IncidentPlus.API
{
    public static class WebApiConfig
    {
        private static String PARAM_DISPLAY_DATA = "data";
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
           // config.EnableCors(cors);
            // Web API configuration and services
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(config.Formatters.JsonFormatter);

            //Apply query string formatters JSON OR XML
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping(PARAM_DISPLAY_DATA, "json", new MediaTypeHeaderValue("application/json")));
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping(PARAM_DISPLAY_DATA, "xml", new MediaTypeHeaderValue("application/xml")));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
