﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        private string Permissions;

        public void SetPermissions(string permissions)
        {
            Permissions = permissions;
        }

        public string GetPermissions()
        {
            return Permissions;
        }
    }
}
