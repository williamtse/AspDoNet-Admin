using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using Admin.IForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.Util.Tree;

namespace Admin.Forms
{
    public class MenuForm:IMenuForm
    {
        private List<Option> Roles;
        private List<Option> Permissions;
        private List<Node> Tree;
        public Form GetForm()
        {
            Form form = new Form();
            form.AddField(new TreeSelect("ParentID", "父级菜单", Tree))
                .AddField(new Text("Title", "名称", "text", true))
                .AddField(new Text("Icon", "图标"))
                .AddField(new Text("Uri", "路径"))
                .AddField(new Text("Order", "排序"));
            form.AddField(new MultipleSelect("Roles", "角色", Roles));
            form.AddField(new Select("Permission", "权限", Permissions));
            return form;
        }

        public void SetPermissions(List<Option> options)
        {
            Roles = options;
        }

        public void SetRoles(List<Option> options)
        {
            Permissions = options;
        }

        public void SetTree(List<Node> tree)
        {
            Tree = tree;
        }
    }
}
