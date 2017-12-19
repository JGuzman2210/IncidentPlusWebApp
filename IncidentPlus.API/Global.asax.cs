using IncidentPlus.API.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace IncidentPlus.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IncidentPlus.API.App_Start.FiltrerConfig.Register(GlobalConfiguration.Configuration);
            CorsConfig.RegisterCors(GlobalConfiguration.Configuration);

            
        }
    }
}
