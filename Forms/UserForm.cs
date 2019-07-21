using Admin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using Admin.IForms;

namespace Admin.Forms
{
    public class UserForm:IUserForm
    {
        private List<Option> Roles;
        private List<Option> Permissions;
        public Form GetForm()
        {
            Form form = new Form();
            form.AddField(new Text("UserName", "用户名", "text", true))
                    .AddField(new Image("Avatar", "头像"))
                    .AddField(new Image("Name", "姓名"))
                    .AddField(new Text("Password", "密码", "password", true))
                    .AddField(new Text("ConfirmPassword", "确认密码", "password", true))
                    .AddField(new MultipleSelect("Roles", "角色", Roles))
                    .AddField(new MultipleSelect("Permissions", "权限", Permissions));
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
    }
}
