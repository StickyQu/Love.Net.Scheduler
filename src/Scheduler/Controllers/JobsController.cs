using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CronExpressionDescriptor;
using Hangfire;
using Hangfire.Storage;
using Love.Net.Scheduler.Internals;
using Love.Net.Scheduler.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace Love.Net.Scheduler.Controllers {
    [Route("")]
    public class JobsController : Controller {
        [HttpGet("jobs/{pageIndex}/{pageSize}")]
        public IActionResult Get([FromRoute] int pageIndex, [FromRoute] int pageSize) {
            using (var connection = JobStorage.Current.GetConnection()) {
                var storageConnection = connection as JobStorageConnection;
                if (storageConnection != null) {
                    int jobCount = (int)storageConnection.GetRecurringJobCount();

                    int startAt = pageIndex * pageSize;

                    int endAt = (int)pageSize;
                    if (endAt == -1) {
                        endAt = jobCount;
                    }
                    else {
                        endAt = startAt + (int)pageSize;
                    }

                    if (startAt > jobCount) {
                        startAt = 0;
                    }
                    if (endAt > jobCount) {
                        endAt = jobCount;
                    }

                    var items = storageConnection.GetRecurringJobs(startAt, endAt)
                        .Select(j => new {
                            j.Id,
                            Job = j.Job.ToString(),
                            Next = j.NextExecution,
                            Last = j.LastExecution,
                            j.Cron,
                            Des = GetDescription(j)
                        });

                    return Ok(items);
                }
                else {
                    var items = connection.GetRecurringJobs()
                        .Select(j => new {
                            j.Id,
                            Job = j.Job.ToString(),
                            Next = j.NextExecution,
                            Last = j.LastExecution,
                            j.Cron,
                            Des = GetDescription(j)
                        });
                    return Ok(items);
                }
            }
        }

        private static string GetDescription(RecurringJobDto j) {
            try {
                return ExpressionDescriptor.GetDescription(j.Cron);
            }
            catch (Exception) {
                //todo
                return j.Cron;
            }
        }

        [HttpPost("jobs")]
        public IActionResult Post([FromBody] CreateDto createDto) {
            RecurringJob.AddOrUpdate(createDto.CallbackUrl, () => ApiCallbackJob.ApiCallback(createDto.CallbackUrl, createDto.Method), createDto.Cron, queue: createDto.Queue);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody] UpdateDto updateDto) {
            try {
                CoreFunc.UpdateCron(updateDto.JobId, updateDto.Cron);
                return Ok("");
            }
            catch (Exception ex) {
                return this.UnprocessableEntity();
            }
        }

        [HttpDelete("job/{id}")]
        public IActionResult Delete([FromRoute] string id) {
            RecurringJob.RemoveIfExists(id);
            return Ok("");
        }
        [HttpGet("cron")]
        public IActionResult GetCron() {
            var items = new[]
                           {
                    new { Name = "Seconds", Required = "Y", Allowed = "0-59", Special = ", - * /" },
                    new { Name = "Minutes", Required = "Y", Allowed = "0-59", Special = ", - * /" },
                    new { Name = "Hours",   Required = "Y", Allowed = "0-23", Special = ", - * /" },
                    new { Name = "Day of month", Required = "Y", Allowed = "1-31", Special = ", - * ? / L W C" },
                    new { Name = "Month", Required = "Y", Allowed = "0-11 or JAN-DEC", Special = ", - * /" },
                    new { Name = "Day of week", Required = "Y", Allowed = "1-7 or SUN-SAT", Special = ", - * ? / L C #" },
                    new { Name = "Year", Required = "N", Allowed = "empty or 1970-2099", Special = ", - * /" },
                };
            return Ok(items);

        }

        [HttpGet("symbol")]
        public IActionResult GetSymbol() {
            var items = new[]
                           {
                    new { Symbol = "*", Meaning = "匹配该域的任意值；如*用在分所在的域，表示每分钟都会触发事件" },
                    new { Symbol = "?", Meaning = "匹配该域的任意值"},
                    new { Symbol = "–", Meaning = "匹配一个特定的范围值；如时所在的域的值是10-12，表示10、11、12点的时候会触发事件"},
                    new { Symbol = ",", Meaning = "匹配多个指定的值；如周所在的域的值是2,4,6，表示在周一、周三、周五就会触发事件(1表示周日，2表示周一，3表示周二，以此类推，7表示周六)"},
                    new { Symbol = "/", Meaning = "左边是开始触发时间，右边是每隔固定时间触发一次事件，如秒所在的域的值是5/15，表示5秒、20秒、35秒、50秒的时候都触发一次事件"},
                    new { Symbol = "L", Meaning = "最后的意思，如果是用在天这个域，表示月的最后一天，如果是用在周所在的域，如6L，表示某个月最后一个周五。（外国周日是星耀日，周一是月耀日，一周的开始是周日，所以1L=周日，6L=周五"},
                    new { Symbol = "W", Meaning = "工作日的意思。如天所在的域的值是15W，表示本月15日最近的工作日，如果15日是周六，触发器将触发上14日周五。如果15日是周日，触发器将触发16日周一。如果15日不是周六或周日，而是周一至周五的某一个，那么它就在15日当天触发事件"},
                    new { Symbol = "#", Meaning = "用来指定每个月的第几个星期几，如6#3表示某个月的第三个星期五"},
                };
            return Ok(items);
        }
    }

    public class UpdateDto {
        public string JobId = "DEBUG";

        public string Cron = "* * * * *";
    }

    public class CreateDto {
        public string Queue { get; set; } = "default";
        public string CallbackUrl { get; set; }
        /// <summary>
        /// GET/POST/PUT/DELETE
        /// </summary>
        public string Method { get; set; } = "GET";
        public string Cron { get; set; } = "* * * * *";
    }
}
