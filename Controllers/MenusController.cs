using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using BootstrapHtmlHelper.Util.Tree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            Form<Menu> form = Form(nodes, new Menu());
            ViewData["form"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            Response.Headers["X-PJAX-URL"] = "/Menus";
            return View();
        }

        private Form<Menu> Form(List<Node> tree, Menu menu)
        {
            Form<Menu> form = new Form<Menu>(menu, (m)=>m.ID);
            form.AddField(new TreeSelect("ParentID", "父级菜单", tree))
                .AddField(new Text("Title", "名称", "text", true))
                .AddField(new Text("Icon", "图标"))
                .AddField(new Text("Uri", "路径"))
                .AddField(new Text("Order", "排序"));

            List<Role> roles = _context.Role.ToList<Role>();
            
            form.AddField(new MultipleSelect("Roles", "角色", Option.GetOptions<Role>(roles, (r) => r.ID.ToString(), (r) => r.Name)));

            List<Permission> permissions = _context.Permission.ToList<Permission>();

            form.AddField(new Select("Permission", "权限", Option.GetOptions<Permission>(permissions, (p)=>p.ID.ToString(), (p)=>p.Name)));

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
                if(Roles!=null)
                await AddRoleMenu(menu.ID, Roles);
            }
            else
            {
                GetErrorListFromModelState(ModelState);
            }
            httpContextAccessor.HttpContext.Response.Redirect($"/Menus/Index");
        }

        private async Task AddRoleMenu(int MenuID, string Roles)
        {
            string[] roleIds = Roles.Split(',');
            foreach (string RoleID in roleIds)
            {
                RoleMenu roleMenu = new RoleMenu();
                roleMenu.MenuID = MenuID;
                roleMenu.RoleID = int.Parse(RoleID);
                _context.Add(roleMenu);
            }
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

            //给Roles赋值
            List<RoleMenu> roleMenus = _context.RoleMenu
                .Where(fullEntity => fullEntity.MenuID == id)
                .ToList<RoleMenu>();
            List<string> roles = new List<string>();
            foreach(RoleMenu rm in roleMenus)
            {
                roles.Add(rm.RoleID.ToString());
            }
            if (roles.Count > 0)
            {
                menu.SetRoles(string.Join(",", roles));
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
                
                List<RoleMenu> roleMenus = _context.RoleMenu.Where<RoleMenu>(rm => rm.MenuID == id).ToList<RoleMenu>();
                _context.RemoveRange(roleMenus);
                if(Roles!=null)
                await AddRoleMenu(id, Roles);

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
