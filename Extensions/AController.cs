using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.Util.Tree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcMovie.Models;
using MvcMovie.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace MvcMovie.Extensions
{
    public class AController : Controller
    {
        protected MvcMovieContext _context;
        protected IHttpContextAccessor httpContextAccessor;

        protected bool isPjax()
        {
            return !string.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Headers["X-PJAX"]);
        }

        

        protected string TreeView()
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
            string path = httpContextAccessor.HttpContext.Request.Path;
            string[] arr = path.Split('/');
            string currentController = '/'+arr[1];
            TreeView treeView = new TreeView(dic, nodes, currentController);
            return treeView.GetContent();
        }

        protected void GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            httpContextAccessor.HttpContext.Session.Set<List<string>>("errors", errorList);
        }

        public IActionResult View()
        {
            if (!isPjax())
            {
                ViewData["treeView"] = TreeView();
                return base.View();
            }
                
            else
                return base.PartialView();
        }

        public IActionResult View(string viewName, object model)
        {
            if (!isPjax())
            {
                ViewData["treeView"] = TreeView();
                return base.View(viewName, model);
            }
            return base.PartialView(viewName, model);
        }
        [NonAction]
        public IActionResult View(object model)
        {
            if (!isPjax())
            {
                ViewData["treeView"] = TreeView();
                return base.View(model);
            }
                
            return base.PartialView(model);
        }
        [NonAction]
        public IActionResult View(string viewName)
        {
            if (!isPjax())
            {
                ViewData["treeView"] = TreeView();
                return base.View(viewName);
            }
            return base.PartialView(viewName);
        }
    }
}
