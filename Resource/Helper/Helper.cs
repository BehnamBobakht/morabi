using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource
{
   public class Helper
    {
        public static string GetLang(string name)
        {
            var value = Resource.Lang.ResourceManager.GetString(name);
            return value;
        }
        public static string GetNotify(string name)
        {
            var value = Resource.Notify.ResourceManager.GetString(name);
            return value;
        }
    }
}
