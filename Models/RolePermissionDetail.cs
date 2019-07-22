using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class RolePermissionDetail:RolePermission
    {
        public string HttpPath;
        public string HttpMethods;
    }
}
