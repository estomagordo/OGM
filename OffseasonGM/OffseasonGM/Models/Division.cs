using OffseasonGM.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Division : IGeographical
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany]
        public List<Team> Teams { get; set; }

        [ManyToOne, ForeignKey(typeof(League))]
        public int LeagueId { get; set; }

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

        public Division()
        {
            Teams = new List<Team>();
        }
       
        public double SquaredDistanceTo(IGeographical other)
        {
            if (!Teams.Any())
            {
                return 0.0;
            }

            return (Latitude - other.Latitude) * (Latitude - other.Latitude) + (Longitude - other.Longitude) * (Longitude - other.Longitude);
        }
    }
}
