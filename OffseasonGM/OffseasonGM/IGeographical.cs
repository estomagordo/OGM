using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM
{
    public interface IGeographical
    {
        double Latitude { get; }
        double Longitude { get; }
        double SquaredDistanceTo(IGeographical other);
    }
}
