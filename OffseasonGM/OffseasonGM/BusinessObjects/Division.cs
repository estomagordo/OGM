using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.BusinessObjects
{
    public class Division
    {
        public List<Team> Teams { get; set; }
        public double AverageLatitude
        {
            get
            {
                return Teams.Average(team => team.City.Latitude);
            }
        }
        public double AverageLongitude
        {
            get
            {
                return Teams.Average(team => team.City.Longitude);
            }
        }
    }
}
