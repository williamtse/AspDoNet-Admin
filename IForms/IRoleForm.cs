using Admin.Interface;
using BootstrapHtmlHelper.FormHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.IForms
{
    public interface IRoleForm: IForm
    {
        void SetPermissions(List<Option> options);
    }
}
