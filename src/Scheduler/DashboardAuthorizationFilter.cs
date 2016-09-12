// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using Hangfire.Dashboard;

namespace Love.Net.Scheduler {
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter {
        public bool Authorize(DashboardContext context) {
            return true;
        }
    }
}