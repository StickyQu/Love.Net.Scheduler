using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Storage;
using Love.Net.Scheduler.Jobs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CronExpressionDescriptor;
using Love.Net.Scheduler.Internals;

namespace Love.Net.Scheduler.Controllers {
    /// <summary>
    ///
    /// </summary>
    [Route("")]
    public class HomeController : Controller {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<RecurringJobDto> Get() {
            using (var connection = JobStorage.Current.GetConnection()) {
                var recurringJobs = connection.GetRecurringJobs();
                return recurringJobs;
            }
        }
    }
}
