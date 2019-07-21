using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Admin.Extensions;
using Admin.Models;
using Admin.Utils;
using Admin.IForms;
using Admin.IViewModels;
using Admin.ViewModels;
using Admin.IService;

namespace Admin.Controllers
{
    public class RolesController : AController
    {
        private IRoleForm _form;
        private readonly IRbacService _rbac;
        private readonly IStringLocalizer<LoginController> Localizer;
        public RolesController(IHttpContextAccessor _httpContextAccessor, 
            AdminContext context, 
            IStringLocalizer<LoginController> localizer,
            IRoleForm roleForm,
            IRbacService rbac)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
            Localizer = localizer;
            _form = roleForm;
            _rbac = rbac;

        }
        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Role.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role
                .FirstOrDefaultAsync(m => m.ID == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        private Form Form()
        {
            List<Permission> permissions = _context.Permission.ToList<Permission>();
            _form.SetPermissions(Option.GetOptions<Permission>(permissions, (p) => p.ID.ToString(), (p) => Localizer[p.Name]));
            return _form.GetForm();
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            Form form = Form();
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Slug,Permissioins")] RoleViewModel roleViewModel)
        {
            Role role = roleViewModel.GetEntity();
            if (ModelState.IsValid)
            {
                _context.Add(role);

                _rbac.AddRolePermissions(role.ID, roleViewModel.Permissions);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel.Permissions = _rbac.GetRolePermissions(role);
            Form form = Form();
            BindObject.CopyModel(roleViewModel, role);
            form.Model(roleViewModel, "ID");
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Slug, Permissions")] RoleViewModel roleViewModel)
        {
            Role role = roleViewModel.GetEntity();
            if (id != role.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    //delete old role permissions
                    //add new role permissions
                    _rbac.RemoveRolePermissions(role);
                    _rbac.AddRolePermissions(role.ID, roleViewModel.Permissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.ID))
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
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role
                .FirstOrDefaultAsync(m => m.ID == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Role.FindAsync(id);
            _context.Role.Remove(role);
            _rbac.RemoveRolePermissions(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.ID == id);
        }
    }
}
