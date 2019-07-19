using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string RememberToken { get; set; }

        private string Roles;

        private string Permissions;

        public void SetRoles(string roles)
        {
            Roles = roles;
        }

        public void SetPermissions(string permissions)
        {
            Permissions = permissions;
        }

        public string GetRoles()
        {
            return Roles;
        }

        public string GetPermissions()
        {
            return Permissions;
        }
    }
}
