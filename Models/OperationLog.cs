using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class OperationLog
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Ip { get; set; }
        public string Input { get; set; }
        [DataType(DataType.DateTime)]
        public  DateTime CreateAt { get; set; }
    }
}
