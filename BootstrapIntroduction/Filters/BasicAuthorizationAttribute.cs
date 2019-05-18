using BootstrapIntroduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootstrapIntroduction.Filters
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var userIdentity = filterContext.HttpContext.User.Identity as User;

            if(userIdentity == null || !userIdentity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();

                return;
            }

            string visitorIPAddress = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(visitorIPAddress)) visitorIPAddress = filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress)) visitorIPAddress = filterContext.HttpContext.Request.UserHostAddress;

            if(userIdentity.ValidIpAddresses != null && !userIdentity.ValidIpAddresses.Contains(visitorIPAddress))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
        }

    }
}