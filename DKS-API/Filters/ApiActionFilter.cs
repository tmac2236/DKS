using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DKS_API.Filters
{
    public class ApiActionFilter : IActionFilter
    {
        //constructor
        public ApiActionFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var reqUrl =  Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.HttpContext.Request);

            //ar header = context.HttpContext.Request.Headers;
            //var querystr = context.HttpContext.Request.QueryString;     
        }
    }

}