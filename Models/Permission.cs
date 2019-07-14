using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Permission
    {
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
