using System;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MvcMovie.Models;
using MvcMovie.Utils;
using Newtonsoft.Json;

namespace MvcMovie.Controllers
{
    public class LoginController : Controller
    {
        private readonly IStringLocalizer<LoginController> _localizer;
        private readonly MvcMovieContext _context;

        public LoginController(IStringLocalizer<LoginController> localizer, MvcMovieContext context)
        {
            _localizer = localizer;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string DoLogin(string Username, string Password, string reurl = "/")
        {
            User user = _context.User.Where(u => u.Username == Username).FirstOrDefault<User>();
            if (user == null)
            {
                return JsonConvert.SerializeObject(new JsonResponse(Error.LOGIN_ERROR, _localizer["Username or password is wrong"]));
            }

            if (Password == null)
            {
                return JsonConvert.SerializeObject(new JsonResponse(Error.LOGIN_ERROR, _localizer["Password is required"]));
            }
            byte[] salt = Convert.FromBase64String(user.Salt);;

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            if(user.Password != hashed)
            {
                return JsonConvert.SerializeObject(new JsonResponse(Error.LOGIN_ERROR, _localizer["Username or password is wrong"]));
            }
            HttpContext.Session.Set<User>("user", user);
            return JsonConvert.SerializeObject(new JsonResponse(reurl));
        }
    }
}