using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            var method = context.HttpContext.Request.Method;
            var actionParam = "GET";
            if(method == "POST" ) actionParam = JsonConvert.SerializeObject(context.ActionArguments.Values, Formatting.Indented);
            _logger.LogInformation("####################################################################");
            _logger.LogInformation(string.Format("ApiActionFilter Client IP : {0} URL : {1} Parameters: {2}",clientIp,reqUrl,actionParam));
        }
    }

}