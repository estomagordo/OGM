﻿using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Season
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int StartYear { get; set; }

        [ForeignKey(typeof(League))]
        public int LeagueId { get; set; }

        [ManyToMany(typeof(SeasonTeam))]
        public List<Team> Teams { get; set; }

        [ManyToMany(typeof(SeasonPlayer))]
        public List<Player> Players { get; set; }

        [OneToMany]
        public List<Goal> Goals { get; set; }

        [OneToMany]
        public List<Match> Matches { get; set; }

        public Season(int startYear)
        {            
            StartYear = startYear;
            Matches = new List<Match>();
            Players = new List<Player>();
            Teams = new List<Team>();
        }

        public override string ToString()
        {
            var shortNextYearString = ((StartYear + 1) % 100).ToString();
            if (shortNextYearString.Length == 1)
            {
                shortNextYearString = "0" + shortNextYearString;
            }

            return StartYear.ToString() + "-" + shortNextYearString;
        }
    }
}
