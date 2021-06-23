using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DKS_API.Filters
{
    public class ApiActionFilter : IActionFilter
    {
        private ILogger<ApiActionFilter> _logger;

        //constructor
        public ApiActionFilter(ILogger<ApiActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var reqUrl =  Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.HttpContext.Request);
            _logger.LogInformation(string.Format(@"###### ApiActionFilter Client IP : {0} ######",clientIp));
            _logger.LogInformation(string.Format(@"###### ApiActionFilter Request URL : {0} ######",reqUrl));
        }
    }

}