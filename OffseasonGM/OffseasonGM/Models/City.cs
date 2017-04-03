using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System;

namespace OffseasonGM.Models
{
    public class City : IGeographical
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [OneToMany]
        public List<Team> Teams { get; set; }

        public double SquaredDistanceTo(IGeographical other)
        {
            return (Latitude - other.Latitude) * (Latitude - other.Latitude) + (Longitude - other.Longitude) * (Longitude - other.Longitude);
        }
    }
}
