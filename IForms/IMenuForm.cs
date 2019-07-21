using BootstrapHtmlHelper.FormHelper;
using Admin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.Util.Tree;

namespace Admin.IForms
{
    public interface IMenuForm:IForm
    {
        
        void SetRoles(List<Option> options);
        void SetPermissions(List<Option> options);
        void SetTree(List<Node> list);
    }
}
