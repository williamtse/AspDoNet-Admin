using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using BootstrapHtmlHelper.Util.Tree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Extensions;
using Admin.Interface;
using Admin.Models;
using Admin.IForms;
using Admin.ViewModels;
using Admin.IService;
using Admin.Utils;

namespace Admin.Controllers
{
    public class MenusController : AController
    {
        private readonly IMenuForm _form;
        private readonly IRbacService _rbac;
        public MenusController(IHttpContextAccessor _httpContextAccessor, 
            AdminContext context,
            IMenuForm menuForm,
            IRbacService rbacService)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
            _form = menuForm;
            _rbac = rbacService;
        }
        // GET: Menus
        public async Task<IActionResult> Index()
        {
            List<Menu> menus = _context.Menu.OrderBy(m => m.Order).ToList<Menu>();
            List<Node> nodes = new List<Node>();
            Dictionary<int, Menu> dic = new Dictionary<int, Menu>();
            foreach (Menu menu in menus)
            {
                Node node = new Node();
                node.ID = menu.ID;
                node.ParentID = menu.ParentID;
                node.Title = menu.Title;
                nodes.Add(node);
                dic.Add(menu.ID, menu);
            }
            MenuNestable nestable = new MenuNestable(dic, nodes, "tree");
            ViewData["menuList"] = nestable.GetContent();
            ViewData["nestableScript"] = nestable.GetScript();

            Form form = Form();
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            Response.Headers["X-PJAX-URL"] = "/Menus";
            return View();
        }

        private Form Form()
        {
            List<Role> roles = _context.Role.ToList<Role>();
            _form.SetPermissions(Option.GetOptions<Role>(roles, (r) => r.ID.ToString(), (r) => r.Name));
            List<Permission> permissions = _context.Permission.ToList<Permission>();
            _form.SetRoles(Option.GetOptions<Permission>(permissions, (p) => p.ID.ToString(), (p) => p.Name));
            List<Menu> menus = _context.Menu.ToList<Menu>();
            _form.SetTree(Builder.ListToNodes<Menu>(menus, (p) => p.ID, (m)=>m.ParentID, (p) => p.Title));
            return _form.GetForm();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([Bind("ID,ParentID,Order,Title,Icon,Uri,Permission,Roles")] MenuViewModel menuViewModel)
        {
            Menu menu = menuViewModel.GetEntity();
            if (ModelState.IsValid)
            {
                _context.Add(menu);

                if (menuViewModel.Roles != null)
                    _rbac.UpdateMenuRoles(menu, menuViewModel.Roles);
                await _context.SaveChangesAsync();
            }
            else
            {
                GetErrorListFromModelState(ModelState);
            }
            Redirect("/Menus/Index");
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            MenuViewModel mvm = new MenuViewModel();
            BindObject.CopyModel(mvm, menu);
            mvm.Roles = _rbac.GetMenuRoles(menu);
            Form form = Form();
            form.Model(mvm, "ID");
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ParentID,Order,Title,Icon,Uri,Permission,Roles")] MenuViewModel menuViewModel)
        {
            Menu menu = menuViewModel.GetEntity();
            if (ModelState.IsValid)
            {
                _context.Update(menu);
                if (menuViewModel.Roles != null)
                    _rbac.UpdateMenuRoles(menu, menuViewModel.Roles);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Menus/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            _context.Menu.Remove(menu);
            _rbac.RemoveMenuRoles(menu);
            await _context.SaveChangesAsync();
            return Json(new JsonResponse());
        }
    }
}
