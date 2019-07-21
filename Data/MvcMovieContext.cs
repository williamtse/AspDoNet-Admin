using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Admin.Models;

namespace Admin.Models
{
    public class AdminContext : DbContext
    {
        public AdminContext (DbContextOptions<AdminContext> options)
            : base(options)
        {
        }

        public DbSet<Admin.Models.User> User { get; set; }

        public DbSet<Admin.Models.Permission> Permission { get; set; }

        public DbSet<Admin.Models.Role> Role { get; set; }

        public DbSet<Admin.Models.Menu> Menu { get; set; }

        public DbSet<Admin.Models.RoleMenu> RoleMenu { get; set; }
        public DbSet<Admin.Models.RolePermission> RolePermission { get; set; }
        public DbSet<Admin.Models.RoleUser> RoleUser { get; set; }
        public DbSet<Admin.Models.UserPermission> UserPermission { get; set; }
    }
}
