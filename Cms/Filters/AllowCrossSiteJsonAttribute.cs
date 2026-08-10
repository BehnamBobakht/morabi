using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace Cms.Api
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var origin = "";
            try { origin = actionExecutedContext.Request.Headers.FirstOrDefault(s => s.Key == "Origin").Value.FirstOrDefault(); }
            catch (Exception ex) { }
            var allowOrigin = !string.IsNullOrWhiteSpace(origin) ? origin : "*";
            
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", allowOrigin);
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}