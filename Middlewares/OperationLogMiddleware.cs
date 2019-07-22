using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Extensions;
using Admin.IService;
using Admin.Models;
using Admin.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Admin.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OperationLogMiddleware
    {
        private readonly RequestDelegate _next;

        public OperationLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            
            User user = httpContext.Session.Get<User>("user");
            if (user != null)
            {
                OperationLog log = new OperationLog();
                log.Method = httpContext.Request.Method;
                log.Ip = httpContext.GetUserIp();
                log.Path = httpContext.Request.Path;
                
                if (httpContext.Request.Form!=null)
                {
                    Dictionary<string, string> form = httpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                    log.Input = JsonConvert.SerializeObject(form);
                }
                log.Username = user.Username;
                log.CreateAt = new DateTime();
                IOperationLogService operationLogService = (IOperationLogService)httpContext.RequestServices.GetService(typeof(IOperationLogService));
                operationLogService.Record(log);
            }
            
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class OperationLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseOperationLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OperationLogMiddleware>();
        }
    }
}
