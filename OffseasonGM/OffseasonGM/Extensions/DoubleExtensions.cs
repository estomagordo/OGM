using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToRoundedString(this double d)
        {
            return ((int)(d + 0.5)).ToString();
        }
    }
}
