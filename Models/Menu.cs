using BootstrapHtmlHelper.Util.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Menu
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Uri { get; set; }
        public string Permission { get; set; }
    }
}
