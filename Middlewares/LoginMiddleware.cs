using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using MvcMovie.Utils;

namespace MvcMovie.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IRouter _router;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            User user = context.Session.Get<User>("user");
            var path = context.Request.Path;
            var query = context.Request.QueryString;
            string pattern = @"\/Login.*";
            if (user == null && !Regex.IsMatch(path, pattern))
            {
                context.Response.Redirect($"/Login?reurl={path}{query}");
            }
            return _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
