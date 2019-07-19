using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Extensions;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class UsersController : AController
    {
        public UsersController(IHttpContextAccessor _httpContextAccessor, MvcMovieContext context)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        protected Form<User> Form(User user)
        {
            Form<User> form = new Form<User>(user, (u)=>u.ID);
            form.AddField(new Text("UserName", "用户名", "text", true))
                .AddField(new Text("Password", "密码", "password", true))
                .AddField(new Text("ConfirmPassword", "确认密码", "password", true))
                .AddField(new MultipleSelect("Roles", "角色", Option.GetOptions<Role>(
                    _context.Role.ToList<Role>(), 
                    (r)=>r.ID.ToString(), 
                    (r)=>r.Name
                    )))
                .AddField(new MultipleSelect("Permissions", "权限", Option.GetOptions<Permission>(
                    _context.Permission.ToList<Permission>(),
                    (r) => r.ID.ToString(),
                    (r) => r.Name
                    )));
            return form;
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            Form<User> form = Form(new Models.User());
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Password,Name,Avatar,RememberToken")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            Form<User> form = Form(user);
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,Password,Name,Avatar,RememberToken")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
