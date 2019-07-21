using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.IViewModels;
using Admin.Utils;

namespace Admin.ViewModels
{
    public class PermissionViewModel : IPermissionViewModel
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
        public Permission GetEntity()
        {
            Permission p = new Permission();
            BindObject.CopyModel(p, this);
            return p;
        }
    }
}
