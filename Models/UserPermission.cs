﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class UserPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PermissionID { get; set; }
    }
}
