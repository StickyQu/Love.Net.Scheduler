// Copyright © xu yingting & top001.com. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Love.Net.Scheduler
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            //var context = new OwinContext(owinEnvironment);

            //// Allow all authenticated users to see the Dashboard.
            //return context.Authentication.User.Identity.IsAuthenticated;
            return true;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            throw new NotImplementedException();
        }
    }
}