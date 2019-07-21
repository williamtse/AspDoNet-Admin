using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class Permission
    {
        public int ID { get; set; }
        //标识
        public string Slug { get; set; }
        //名称
        public string Name { get; set; }

        //HTTP方法
        public string HttpMethods { get; set; }

        //HTTP路径
        public string HttpPath { get; set; } 
    }
}
