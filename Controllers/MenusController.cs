using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.UI;
using BootstrapHtmlHelper.Util.Tree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Extensions;
using MvcMovie.Models;


namespace MvcMovie.Controllers
{
    public class MenusController : AController
    {
        public MenusController(IHttpContextAccessor _httpContextAccessor, MvcMovieContext context)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
        }
        // GET: Menus
        public async Task<IActionResult> Index()
        {
            List<Menu> menus = _context.Menu.OrderBy(m=>m.Order).ToList<Menu>();
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

            Form<Menu> form = Form(nodes);
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            Response.Headers["X-PJAX-URL"] = "/Menus";
            return View();
        }

        private Form<Menu> Form(List<Node> tree, Menu menu=null)
        {
            Form<Menu> form = new Form<Menu>();
            if (menu != null)
            {
                form.Edit(menu);
                form.Method("Put");
                form.Action("/Menus/Edit/"+menu.ID.ToString());
            }
            else
            {
                form.Action("/Menus/Create");
            }

            form.TreeSelect("ParentID", "父级菜单", tree);
            form.Text("Title", "名称");
            form.Text("Icon", "图标");
            form.Text("Uri", "路径");
            form.Text("Order", "排序");
            List<Role> roles = _context.Role.ToList<Role>();
            List<Option> options = new List<Option>();
            foreach(Role role in roles)
            {
                options.Add(new Option { value = role.ID.ToString(), text = role.Name });
            }
            form.MultipleSelect("Roles", "角色", options);

            List<Permission> permissions = _context.Permission.ToList<Permission>();
            List<Option> options2 = new List<Option>();
            foreach (Permission permission in permissions)
            {
                options2.Add(new Option { value = permission.ID.ToString(), text = permission.Name });
            }
            form.Select("Permission", "权限", options2);
            return form;
        }
        
        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([Bind("ID,ParentID,Order,Title,Icon,Uri,Permission")] Menu menu, string Roles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
            }
            httpContextAccessor.HttpContext.Response.Redirect($"/Menus/Index");
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

            List<Menu> menus = _context.Menu.ToList<Menu>();
            List<Node> nodes = new List<Node>();
            Dictionary<int, Menu> dic = new Dictionary<int, Menu>();
            foreach (Menu m in menus)
            {
                Node node = new Node();
                node.ID = m.ID;
                node.ParentID = m.ParentID;
                node.Title = m.Title;
                nodes.Add(node);
                dic.Add(m.ID, menu);
            }
            Form<Menu> form = Form(nodes, menu);
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ParentID,Order,Title,Icon,Uri,Permission")] Menu menu, string Roles)
        {
            if (ModelState.IsValid)
            {
                    _context.Update(menu);
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
            await _context.SaveChangesAsync();
            return Json(new JsonResponse());
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.ID == id);
        }
    }
}
