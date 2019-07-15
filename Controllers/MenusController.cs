using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Nestable nestable = new Nestable(nodes, "tree", (node)=> {
                Menu menu = dic[node.ID];
                return "<i class='fa "+ menu.Icon + "'></i><strong>" + menu.Title + "</strong>"
                + "<a href='" + menu.Uri + "' class='dd-nodrag'>" + menu.Uri + "</a>"
                + "<span class=\"pull-right dd-nodrag\">"
                + "<a href = \"/Menus/Edit/" + menu.ID + "\" ><i class=\"fa fa-edit\"></i></a>"
                + "<a href = \"javascript:void(0);\" data-id=\"" + menu.ID + "\" class=\"tree_branch_delete\"><i class=\"fa fa-trash\"></i></a>"
                + "</span>"
                ;
            });
            ViewData["menuList"] = nestable.GetContent();
            return View();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ParentID,Order,Title,Icon,Uri,Permission")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
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
            return View(menu);
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

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.ID == id);
        }
    }
}
