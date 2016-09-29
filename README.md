# Love.Net.Scheduler

[![Join the chat at https://gitter.im/lovedotnet/Love.Net.Scheduler](https://badges.gitter.im/lovedotnet/Love.Net.Scheduler.svg)](https://gitter.im/lovedotnet/Love.Net.Scheduler?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This repo contains the WYSWYG task scheduler based on [Hangfire](http://hangfire.io/) implementation for Asp.Net Core.

# Usage

- copy  files in project Host
- Install-Package Love.Net.Scheduler
- Change config in file Startup.cs
```csharp
//startup.cs
        public void Configure(IApplicationBuilder app) {

            //culture info
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            // redis connection string
            GlobalConfiguration.Configuration.UseStorage(new RedisStorage("127.0.0.1:6379"));

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();
            app.UseDefaultFiles();
        }

```

# view
- open [http://localhost:5000]()


# Authorization

```csharp
            app.UseHangfireDashboard(options: new DashboardOptions() {
                Authorization = new List<IDashboardAuthorizationFilter>() { new DashboardAuthorizationFilter() }
            });
```
