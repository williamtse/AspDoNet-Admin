using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class RoleMenu
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int MenuID { get; set; }
    }
}
