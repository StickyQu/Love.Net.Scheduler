// Copyright © RigoFunc (xuyingting). All rights reserved.

using System;
using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;

namespace Love.Net.Scheduler.Internals {
    internal class CoreFunc {
        public static void UpdateCron(string jobId, string cronExpression) {
            if (string.IsNullOrEmpty(jobId))
                return;

            using (var connection = JobStorage.Current.GetConnection()) {
                var hash = connection.GetAllEntriesFromHash(String.Format("recurring-job:{0}", jobId));
                if (hash == null) {
                    return;
                }

                var job = JobHelper.FromJson<InvocationData>(hash["Job"]).Deserialize();

                new RecurringJobManager().AddOrUpdate(jobId, job, cronExpression);
            }
        }
    }
}
