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
    public class PermissionsController : AController
    {
        public PermissionsController(IHttpContextAccessor _httpContextAccessor, MvcMovieContext context)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
        }

        // GET: Permissions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Permission.ToListAsync());
        }

        // GET: Permissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission
                .FirstOrDefaultAsync(m => m.ID == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        public Form<Permission> Form(Permission permission)
        {
            List<Option> options = new List<Option>();

            options.Add(new Option { text = "GET", value = "GET" });
            options.Add(new Option { text = "POST", value = "POST" });
            options.Add(new Option { text = "PUT", value = "PUT" });
            options.Add(new Option { text = "DELET", value = "DELET" });
            options.Add(new Option { text = "CONNECT", value = "CONNECT" });
            options.Add(new Option { text = "OPTIONS", value = "OPTIONS" });
            options.Add(new Option { text = "TRACE", value = "TRACE" });
            options.Add(new Option { text = "PATCH", value = "PATCH" });

            Form<Permission> form = new Form<Permission>(permission, (m) => m.ID);
            form.AddField(new Text("Slug", "标识", "text", true));
            form.AddField(new Text("Name", "名称", "text", true));
            form.AddField(new MultipleSelect("HttpMethods", "Http方法", options));
            form.AddField(new Textarea("HttpPath", "Http路径"));
            return form;
        }

        // GET: Permissions/Create
        public IActionResult Create()
        {
            Form<Permission> form = Form(new Permission());
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Slug,Name,HttpMethods,HttpPath")] Permission permission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permission);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            Form<Permission> form = Form(permission);
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View(permission);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Slug,Name,HttpMethods,HttpPath")] Permission permission)
        {
            if (id != permission.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionExists(permission.ID))
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
            return View(permission);
        }

        // GET: Permissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permission
                .FirstOrDefaultAsync(m => m.ID == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permission.FindAsync(id);
            _context.Permission.Remove(permission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionExists(int id)
        {
            return _context.Permission.Any(e => e.ID == id);
        }
    }
}
