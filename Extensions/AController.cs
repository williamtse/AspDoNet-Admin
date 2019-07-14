using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Extensions
{
    public class AController : Controller
    {
        protected readonly IHttpContextAccessor httpContextAccessor;

        public AController(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }
        public IActionResult View()
        {
            if (string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-PJAX"]))
                return base.View();
            else
                return base.PartialView();
        }

        public IActionResult View(string viewName, object model)
        {
            if (string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-PJAX"]))
                return base.View(viewName, model);
            else
                return base.PartialView(viewName, model);
        }
        [NonAction]
        public IActionResult View(object model)
        {
            if (string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-PJAX"]))
                return base.View(model);
            else
                return base.PartialView(model);
        }
        [NonAction]
        public IActionResult View(string viewName)
        {
            if (string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-PJAX"]))
                return base.View(viewName);
            else
                return base.PartialView(viewName);
        }
    }
}
