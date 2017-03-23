using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Extensions
{
    public static class RandomExtensions
    {
        public static double NextGaussian(this Random random)
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();

            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}