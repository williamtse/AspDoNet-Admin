using Admin.Attributes;
using Admin.Interface;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Admin.Utils;
using Admin.IViewModels;

namespace Admin.ViewModels
{
    public class UserViewModel:Object,IUserViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        [UniqueUserName]
        public string Username { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "确认密码不能为空")]
        [Compare("Password", ErrorMessage = "与密码不一致")]
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string RememberToken { get; set; }
        public string Roles { get; set; }
        public string Permissions { get; set; }

        public User GetEntity()
        {
            User user = new User();
            BindObject.CopyModel(user, this);
            return user;
        }

        public void SetEnity(User entity)
        {
            
        }
    }
}
