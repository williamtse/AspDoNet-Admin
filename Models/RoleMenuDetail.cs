using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class RoleMenuDetail
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int MenuID { get; set; }
        public string RoleName { get; set; }
        public string MenuTitle { get; set; }
    }
}
