using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Redis;
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

            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            GlobalConfiguration.Configuration.UseStorage(new RedisStorage("127.0.0.1:6379"));

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();
            app.UseDefaultFiles();
        }
    }
}
