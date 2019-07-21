using BootstrapHtmlHelper.FormHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Interface
{
    public interface IForm:IDependency
    {
        Form GetForm();
    }
}
