using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AdminContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AdminContext>>()))
            {
                // Look for any movies.
                if (context.User.FirstOrDefault()!=null)
                {
                    return;   // DB has been seeded
                }

                //添加管理员
                context.User.AddRange(
                    new User
                    {
                        Username = "admin",
                        Password = "/OOoOer10+tGwTRDTrQSoeCxVTFr6dtYly7d0cPxIak=",
                        Salt = "NZsP6NnmfBuYeJrrAKNuVQ==",
                        Name = "管理员"
                    }
                );
                
                //添加管理员角色
                context.Role.Add(new Role { Name = "Administrator", Slug= "Administrator" });

                //添加管理菜单
                context.Menu.AddRange(
                    new Menu
                    {
                        Title = "Dashboard",
                        Order = 0,
                        ParentID = 0,
                        Icon = "fa-dashboard",
                        Uri = "/"
                    }  ,
                    new Menu
                    {
                        Title = "Admin",
                        Order = 1,
                        ParentID = 0,
                        Icon = "fa-tasks"
                    },
                    new Menu
                    {
                        Title = "Users",
                        Order = 0,
                        ParentID = 2,
                        Icon = "fa-users",
                        Uri = "/Users"
                    },
                    new Menu
                    {
                        Title = "Roles",
                        Order = 1,
                        ParentID = 2,
                        Icon = "fa-user",
                        Uri = "/Roles"
                    },
                    new Menu
                    {
                        Title = "Permission",
                        Order = 2,
                        ParentID = 2,
                        Icon = "fa-ban",
                        Uri = "/Permissions"
                    },
                    new Menu
                    {
                        Title = "Menu",
                        Order = 3,
                        ParentID = 2,
                        Icon = "fa-list",
                        Uri = "/Menus"
                    },
                    new Menu
                    {
                        Title = "Operation log",
                        Order = 4,
                        ParentID = 2,
                        Icon = "fa-history",
                        Uri = "/OperationLogs"
                    }
                );

                //添加权限
                context.Permission.AddRange(
                    new Permission
                    {
                        Slug="*",
                        Name="All permission",
                        HttpPath="/*"
                    },
                    new Permission
                    {
                        Slug="dashboard",
                        Name="Dashboard",
                        HttpMethods="GET",
                        HttpPath="/"
                    },
                    new Permission
                    {
                        Slug="auth.login",
                        Name="Login",
                        HttpPath="/Login,/Login/Logout"
                    },
                    new Permission
                    {
                        Slug="auth.setting",
                        Name="User setting",
                        HttpMethods="GET,PUT",
                        HttpPath="/Auth/Setting"
                    },
                    new Permission
                    {
                        Slug="auth.management",
                        Name="Auth management",
                        HttpPath="/Roles,/Permissions,/Menu,/Logs"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
