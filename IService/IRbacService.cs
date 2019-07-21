using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Interface;
using Admin.Models;
using Admin.ViewModels;

namespace Admin.IService
{
    public interface IRbacService:IDependency
    {
        void DeleteUserRoles(User user);
        void AddUserRoles(User user, string Roles);
        void UpdateUserRoles(User user, string Roles);
        void AddRolePermissions(int roleId, string Permissions);
        string GetUserPermissions(User user);
        string GetUserRoles(User user);
        /// <summary>
        /// 删除用户的权限信息
        /// </summary>
        /// <param name="user"></param>
        void RemoveUserAuthorities(User user);
        void AddUserPermissions(User user, string Permissions);
        string GetRolePermissions(Role role);
        void RemoveRolePermissions(Role role);
        void UpdateMenuRoles(Menu menu, string roles);
        string GetMenuRoles(Menu menu);
        void RemoveMenuRoles(Menu menu);
    }
}
