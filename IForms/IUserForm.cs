using BootstrapHtmlHelper.FormHelper;
using Admin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.IForms
{
    public interface IUserForm: IForm
    {
        void SetRoles(List<Option> options);
        void SetPermissions(List<Option> options);
    }
}
