using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;
using NLog.Config;
using NLog.Internal;
using NLog.Targets;

namespace ConcurrencySandbox
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AttachLogging();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void AttachLogging()
        {
            var syslogUrl = System.Configuration.ConfigurationManager.AppSettings["SyslogUrl"];
            var syslogPort = System.Configuration.ConfigurationManager.AppSettings["SyslogPort"];

            int port;
            int.TryParse(syslogPort, out port);

            var target = new Syslog()
            {
                SyslogServer = syslogUrl,
                Port = port,
                Facility = SyslogFacility.Local7,
                Name = "syslog"
            };

            LogManager.Configuration.AddTarget("syslog", target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
            LogManager.Configuration.Reload();
        }
    }
}
