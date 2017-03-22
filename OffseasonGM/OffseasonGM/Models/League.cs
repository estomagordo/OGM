using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Models
{
    public class League
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastChanged { get; set; }

        [OneToMany]
        public List<Team> Teams { get; set; }
    }
}
