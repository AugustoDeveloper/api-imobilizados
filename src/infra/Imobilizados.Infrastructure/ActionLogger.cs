using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;

namespace Imobilizados.Infrastructure
{
    public class ActionLogger : IActionFilter
    {
        private ILogger<ActionLogger> logger;

        public ActionLogger(ILogger<ActionLogger> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var data = new 
            {
                Version = "1.0",
                User = context.HttpContext.User.Identity.Name,
                IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Hostname = context.HttpContext.Request.Host.ToString(),
                AreaAccessed = context.HttpContext.Request.GetDisplayUrl(),
                Action = context.ActionDescriptor.DisplayName,
                Timestamp = DateTime.Now
            };

            logger.LogInformation(1, data.ToString());
            logger.LogTrace("Passei por aqui");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
