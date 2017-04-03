using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.BusinessObjects
{
    public class Division : IGeographical
    {
        public List<Team> Teams { get; set; }
        public double Latitude
        {
            get
            {
                return Teams.Average(team => team.City.Latitude);
            }
        }
        public double Longitude
        {
            get
            {
                return Teams.Average(team => team.City.Longitude);
            }
        }
       
        public double SquaredDistanceTo(IGeographical other)
        {
            return (Latitude - other.Latitude) * (Latitude - other.Latitude) + (Longitude - other.Longitude) * (Longitude - other.Longitude);
        }
    }
}
