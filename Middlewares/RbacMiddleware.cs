using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.IService;
using Admin.Models;
using Admin.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Admin.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RbacMiddleware
    {
        private readonly RequestDelegate _next;

        public RbacMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            User user = httpContext.Session.Get<User>("user");
            IRbacService rbac = (IRbacService) httpContext.RequestServices.GetService(typeof(IRbacService));
            HttpRequest path = httpContext.Request;
            if (!rbac.Check(user, path))
            {
                await httpContext.Response.WriteAsync(@"禁止访问");
            }
            else
            {
                await _next(httpContext);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RbacMiddlewareExtensions
    {
        public static IApplicationBuilder UseRbacMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RbacMiddleware>();
        }
    }
}
