using Admin.IViewModels;
using Admin.Models;
using Admin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class MenuViewModel : IMenuViewModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Uri { get; set; }
        public string Permission { get; set; }

        public string Roles { get; set; }
        public Menu GetEntity()
        {
            Menu menu = new Menu();
            BindObject.CopyModel(menu, this);
            return menu;
        }
    }
}
