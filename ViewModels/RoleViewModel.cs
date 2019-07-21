using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.Util;
using Admin.IViewModels;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Admin.Utils;

namespace Admin.ViewModels
{
    public class RoleViewModel : IRoleViewModel
    {
        
        public int ID { get; set; }
        //标识
        [Required(ErrorMessage = "标识不能为空")]
        public string Slug { get; set; }
        //名称
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        public string Permissions { get; set; }
        public Role GetEntity()
        {
            Role role = new Role();
            BindObject.CopyModel(role, this);
            return role;
        }
    }
}
