using System;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    [RouteAttribute("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet()]
        public string Get()
        {
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromDays(1));
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Daily Job"), Cron.Daily);
            return BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));
        }
    }
}