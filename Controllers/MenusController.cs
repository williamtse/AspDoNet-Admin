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
        private readonly MvcMovieContext _context;

        public MenusController(IHttpContextAccessor _httpContextAccessor, MvcMovieContext context) : base(_httpContextAccessor)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            List<Menu> menus = _context.Menu.ToList<Menu>();
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

            return View();
        }

        private Form<Menu> Form(List<Node> tree, Menu menu=null)
        {
            Form<Menu> form = new Form<Menu>();
            if (menu != null)
            {
                form.Edit(menu);
                form.Action("/Menus/Edit");
<<<<<<< HEAD
            }
            else
            {
                form.Action("/Menus/Index");
            }

=======
            }
            else
            {
                form.Action("/Menus/Create");
            }
>>>>>>> 4f9506f35d53fa8705af9295d5a7d246a6d5c6d4
            form.TreeSelect("ParentID", "父级菜单", tree);
            form.Text("Title", "名称");
            form.Text("Icon", "图标");
            form.Text("Uri", "路径");
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
        
        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu
                .FirstOrDefaultAsync(m => m.ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task Create([Bind("ID,ParentID,Order,Title,Icon,Uri,Permission")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
            }
            httpContextAccessor.HttpContext.Response.Redirect($"/Menus/Index");
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ParentID,Order,Title,Icon,Uri,Permission")] Menu menu)
        {
            if (id != menu.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.ID))
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
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
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
