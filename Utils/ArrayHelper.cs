using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Admin.Utils
{
    public class ArrayHelper
    {
        public static List<int> GetFieldsInt<T>(List<T> list, Expression<Func<T, int>> exp)
        {
            var func = exp.Compile();
            var ret = new List<int>();
            foreach (T t in list)
            {
                var v = func(t);
                ret.Add(v);
            }
            return ret;
        }

        public static List<string> GetFieldsString<T>(List<T> list, Expression<Func<T, string>> exp)
        {
            var func = exp.Compile();
            var ret = new List<string>();
            foreach (T t in list)
            {
                var v = func(t);
                ret.Add(v);
            }
            return ret;
        }
    }
}
