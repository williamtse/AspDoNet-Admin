using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Extensions
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            HttpContext httpContext = context.HttpContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("action执行之前");
        }
    }
}
