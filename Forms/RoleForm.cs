using Admin.IForms;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Forms
{
    public class RoleForm : IRoleForm
    {
        private List<Option> Permissions;
        public Form GetForm()
        {
            Form form = new Form();

            form.AddField(new Text("Slug", "标识", "text", true));
            form.AddField(new Text("Name", "名称", "text", true));
            form.AddField(new MultipleSelect("Permissions", "权限", Permissions));
            return form;
        }

        public void SetPermissions(List<Option> options)
        {
            Permissions = options;
        }
    }
}
