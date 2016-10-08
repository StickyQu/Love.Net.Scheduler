using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis;
using Love.Net.Scheduler;

namespace ConsoleApplication {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddHangfire(options => { });
            // Add framework services.
            services.AddMvc();

            services.AddLogging();

            // Add our repository type
        }
        public void Configure(IApplicationBuilder app) {

            app.UseDefaultFiles();
            app.UseStaticFiles();
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            GlobalConfiguration.Configuration.UseStorage(new RedisStorage("127.0.0.1:6379"));

            app.UseHangfireDashboard(options: new DashboardOptions() {
                Authorization = new List<IDashboardAuthorizationFilter>() { new DashboardAuthorizationFilter() }
            });
            app.UseHangfireServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
