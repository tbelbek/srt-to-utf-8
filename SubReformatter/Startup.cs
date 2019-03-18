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
            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => HomeController.ConvertFiles(), Cron.Hourly);
        }
    }
}