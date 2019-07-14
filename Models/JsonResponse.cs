using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class JsonResponse<T>
    {
        public int code { get; set; }
        public string message { get; set; }

        public List<T> data { get; set; }
    }

    public class JsonResponse
    {
        public int code { get; set; }
        public string message { get; set; }

        public string reurl { get; set; }

        public JsonResponse(int _code, string _message)
        {
            code = _code;
            message = _message;
        }

        public JsonResponse()
        {
            code = 0;
            message = "success";
        }

        public JsonResponse(string _reurl)
        {
            code = 0;
            message = "success";
            reurl = _reurl;
        }
    }

}
