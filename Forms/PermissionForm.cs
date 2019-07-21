using Admin.IForms;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Forms
{
    public class PermissionForm : IPermissionForm
    {
        public Form GetForm()
        {
            List<Option> options = new List<Option>();

            options.Add(new Option { text = "GET", value = "GET" });
            options.Add(new Option { text = "POST", value = "POST" });
            options.Add(new Option { text = "PUT", value = "PUT" });
            options.Add(new Option { text = "DELET", value = "DELET" });
            options.Add(new Option { text = "CONNECT", value = "CONNECT" });
            options.Add(new Option { text = "OPTIONS", value = "OPTIONS" });
            options.Add(new Option { text = "TRACE", value = "TRACE" });
            options.Add(new Option { text = "PATCH", value = "PATCH" });

            Form form = new Form();
            form.AddField(new Text("Slug", "标识", "text", true));
            form.AddField(new Text("Name", "名称", "text", true));
            form.AddField(new MultipleSelect("HttpMethods", "Http方法", options));
            form.AddField(new Textarea("HttpPath", "Http路径"));
            return form;
        }
    }
}
