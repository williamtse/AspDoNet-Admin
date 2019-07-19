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
using MvcMovie.Extensions;
using MvcMovie.Models;
using MvcMovie.Utils;

namespace MvcMovie.Controllers
{
    public class RolesController : AController
    {
        private readonly IStringLocalizer<LoginController> Localizer;
        public RolesController(IHttpContextAccessor _httpContextAccessor, MvcMovieContext context, IStringLocalizer<LoginController> localizer)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
            Localizer = localizer;
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

        

        private Form<Role> Form(Role role)
        {
            Form<Role> form = new Form<Role>(role, (m) => m.ID);

            form.AddField(new Text("Slug", Localizer["Slug"], "text" ,true));
            form.AddField(new Text("Name", Localizer["Name"], "text", true));
            List<Permission> permissions = _context.Permission.ToList<Permission>();
            form.AddField(new MultipleSelect("Permissions", Localizer["Permissions"], Option.GetOptions<Permission>(permissions, (p) => p.ID.ToString(), (p) => Localizer[p.Name])));
            return form;
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            Form<Role> form = Form(new Role());
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        private void AddRolePermissions(int roleId, string Permissions)
        {
            List<RolePermission> rps = new List<RolePermission>();
            foreach (string PermissionID in Permissions.Split(','))
            {
                rps.Add(new RolePermission
                {
                    RoleID = roleId,
                    PermissionID = int.Parse(PermissionID)
                });
            }
            if (rps.Count > 0)
                _context.RolePermission.AddRange(rps);
        }
        
        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Slug")] Role role, string Permissions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);

                AddRolePermissions(role.ID, Permissions);

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

            List<RolePermission> rolePermissions = _context.RolePermission
                .Where((rp) => rp.RoleID == id)
                .ToList<RolePermission>();

            if(rolePermissions.Count>0)
                role.SetPermissions(String.Join(',', ArrayHelper.GetFieldsString<RolePermission>(rolePermissions, (rp)=>rp.ID.ToString())));

            Form<Role> form = Form(role);
            
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Slug")] Role role, string Permissions)
        {
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
                    List<RolePermission> old_rps = _context.RolePermission.Where((rp)=>rp.RoleID==id).ToList<RolePermission>();
                    _context.RemoveRange(old_rps);
                    //add new role permissions
                    AddRolePermissions(role.ID, Permissions);
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
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.ID == id);
        }
    }
}
