using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class RolePermission
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
    }
}
