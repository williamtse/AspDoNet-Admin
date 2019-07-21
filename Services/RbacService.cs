using Microsoft.AspNetCore.Http;
using Admin.Extensions;
using Admin.IService;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.ViewModels;
using Admin.Utils;

namespace Admin.Services
{
    public class RbacService:IRbacService
    {
        private readonly AdminContext _context;
        public RbacService()
        {
            _context = XWFHttpContext.ServiceProvider.GetService(typeof(AdminContext)) as AdminContext;
        }
        public void DeleteUserRoles(User user)
        {
            List<RoleUser> roleUsers = _context.RoleUser.Where((ru) => ru.UserID == user.ID).ToList<RoleUser>();
            if(roleUsers.Count>0)
            _context.RemoveRange(roleUsers);
        }

        public void AddUserRoles(User user, string Roles)
        {
            if(Roles != null)
            {
                string[] roles = Roles.Split(',');
                List<RoleUser> rus = new List<RoleUser>();
                foreach (string role in roles)
                {
                    rus.Add(new RoleUser { RoleID = int.Parse(role), UserID = user.ID });
                }
                _context.RoleUser.AddRange(rus);
            }
        }

        public void AddUserPermissions(User user, string Permissions)
        {
            if (Permissions != null)
            {
                string[] permissions = Permissions.Split(',');
                List<UserPermission> rus = new List<UserPermission>();
                foreach (string permission in permissions)
                {
                    rus.Add(new UserPermission { PermissionID = int.Parse(permission), UserID = user.ID });
                }
                _context.UserPermission.AddRange(rus);
            }
        }

        public void UpdateUserRoles(User user, string Roles)
        {
            DeleteUserRoles(user);
            AddUserRoles(user, Roles);
        }

        public void AddRolePermissions(int roleId, string Permissions)
        {
            List<RolePermission> rps = new List<RolePermission>();
            foreach (string PermissionID in Permissions.Split(','))
            {
                rps.Add(new RolePermission
                {
                    RoleID = roleId,
                    PermissionID = int.Parse(PermissionID)
                });
            }
            if (rps.Count > 0)
                _context.RolePermission.AddRange(rps);
        }

        public void SetUserAuthorities(UserViewModel user)
        {
            //获取用户角色
            List<RoleUser> roles = _context.RoleUser.Where((ru) => ru.UserID == user.ID).ToList<RoleUser>();
            user.Roles = string.Join(',', ArrayHelper.GetFieldsString<RoleUser>(roles, (ur) => ur.RoleID.ToString()));
            //获取用户权限
            List<UserPermission> ups = _context.UserPermission.Where((ru) => ru.UserID == user.ID).ToList<UserPermission>();
            user.Permissions = string.Join(',', ArrayHelper.GetFieldsString<RoleUser>(roles, (ur) => ur.RoleID.ToString()));
        }

        public void RemoveUserAuthorities(User user)
        {
            List<RoleUser> roles = _context.RoleUser.Where((ru) => ru.UserID == user.ID).ToList<RoleUser>();
            _context.RemoveRange(roles);
            List<UserPermission> ups = _context.UserPermission.Where((ru) => ru.UserID == user.ID).ToList<UserPermission>();
            _context.RemoveRange(ups);
        }

        public string GetUserPermissions(User user)
        {
            List<UserPermission> ups = _context.UserPermission.Where((u) => u.UserID == user.ID).ToList<UserPermission>();
            if (ups.Count > 0)
                return string.Join(',', ArrayHelper.GetFieldsString(ups, (u) => u.PermissionID.ToString()));
            else
                return null;
        }

        public string GetUserRoles(User user)
        {
            List<RoleUser> ups = _context.RoleUser.Where((u) => u.UserID == user.ID).ToList<RoleUser>();
            if (ups.Count > 0)
                return string.Join(',', ArrayHelper.GetFieldsString(ups, (u) => u.RoleID.ToString()));
            else
                return null;
        }

        public string GetRolePermissions(Role role)
        {
            List<RolePermission> ups = _context.RolePermission.Where((u) => u.RoleID == role.ID).ToList<RolePermission>();
            if (ups.Count > 0)
                return string.Join(',', ArrayHelper.GetFieldsString(ups, (u) => u.PermissionID.ToString()));
            else
                return null;
        }

        public void RemoveRolePermissions(Role role)
        {
            List<RolePermission> rps = _context.RolePermission.Where((rp) => rp.RoleID == role.ID).ToList<RolePermission>();
            _context.RemoveRange(rps);
        }

        public void UpdateMenuRoles(Menu menu, string roles)
        {
            if (roles != null)
            {
                List<RoleMenu> rps = _context.RoleMenu.Where((rp) => rp.MenuID == menu.ID).ToList<RoleMenu>();
                _context.RemoveRange(rps);
                List<RoleMenu> news = new List<RoleMenu>();
                foreach (string role in roles.Split(','))
                {
                    news.Add(new RoleMenu {
                        MenuID=menu.ID,
                        RoleID=int.Parse(role)
                    });
                }
                _context.AddRange(news);
            }
        }

        public string GetMenuRoles(Menu menu)
        {
            List<RoleMenu> rps = _context.RoleMenu.Where((rp) => rp.MenuID == menu.ID).ToList<RoleMenu>();
            return string.Join(',', ArrayHelper.GetFieldsString(rps, (rp) => rp.RoleID.ToString()));
        }

        public void RemoveMenuRoles(Menu menu)
        {
            List<RoleMenu> rps = _context.RoleMenu.Where((rp) => rp.MenuID == menu.ID).ToList<RoleMenu>();
            if(rps.Count>0)
            _context.RemoveRange(rps);
        }
    }
}
