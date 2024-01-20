using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Misc
{
    public static class SomeExtensions
    {
        public static bool IsValid(this string str)
        {
            if (str == null) return false;
            if (string.IsNullOrEmpty(str)) return false;

            return true;
        }
    }
}
