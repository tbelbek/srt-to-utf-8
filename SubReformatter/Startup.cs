using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Hangfire;
using Hangfire.MemoryStorage;

using Owin;

using SubReformatter.Controllers;

namespace SubReformatter
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();
            // Map Dashboard to the `http://<your-app>/hangfire` URL.
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => HomeController.ConvertFiles(), Cron.Hourly);
        }
    }
}