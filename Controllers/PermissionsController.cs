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
using Admin.Extensions;
using Admin.Models;
using Admin.IForms;
using Admin.ViewModels;
using Admin.Utils;

namespace Admin.Controllers
{
    public class PermissionsController : AController
    {
        private readonly IPermissionForm _form;
        public PermissionsController(IHttpContextAccessor _httpContextAccessor, 
            AdminContext context,
            IPermissionForm form)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
            _form = form;
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

        public Form Form()
        {
            return _form.GetForm();
        }

        // GET: Permissions/Create
        public IActionResult Create()
        {
            Form form = Form();
            form.Model(new PermissionViewModel(), "ID");
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
            PermissionViewModel pvm = new PermissionViewModel();
            BindObject.CopyModel(pvm, permission);
            Form form = Form();
            form.Model(pvm, "ID");
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View(permission);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Slug,Name,HttpMethods,HttpPath")] PermissionViewModel permissionViewModel)
        {
            Permission permission = permissionViewModel.GetEntity();
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
            else
            {
                GetErrorListFromModelState(ModelState);
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
