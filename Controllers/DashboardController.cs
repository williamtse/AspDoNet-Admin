using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Extensions;
using Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class DashboardController : AController
    {
        public DashboardController(IHttpContextAccessor _httpContextAccessor,
            AdminContext context)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}