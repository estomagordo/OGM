using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Global
{
    public class GlobalObjects
    {
        private static Random _random;

        public static Random Random
        {
            get
            {
                return _random ?? (_random = new Random());
            }
        }
    }
}
