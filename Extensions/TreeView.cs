using BootstrapHtmlHelper.UI;
using BootstrapHtmlHelper.Util.Tree;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Extensions
{
    public class TreeView : NavTreeView
    {
        private Dictionary<int, Menu> _dic;
        private string _currentUri;
        public TreeView(Dictionary<int, Menu> dic, List<Node> nodes, string currentUri)
        {
            _dic = dic;
            Builder builder = new Builder(nodes);
            _nodes = builder.getTree();
            _currentUri = currentUri;
        }
        protected override string _handle(Node node)
        {
            Menu menu = _dic[node.ID];
            bool hasSubItems = node.SubItems != null && node.SubItems.Count > 0;
            string link = @"<a href='" + (hasSubItems ? "#" : menu.Uri) + @"' 
                                class='nav-link " + ( _currentUri==menu.Uri?"active":"" ) + @"'>
                                <i class='nav-icon fa " + menu.Icon + @"'></i>
                                <p>
                                    " + node.Title + @"
                                    " + (hasSubItems ? "<i class='right fa fa-angle-left'></i>" : "") + @"
                                </p>
                            </a>";
            return link;
        }
    }
}
