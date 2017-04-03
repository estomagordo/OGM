using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Models
{
    public class League
    {
        public enum LeagueConfiguration
        {
            Teams30Divisions4,
            Teams30Divisions6,
            Teams31Divisions4,
            Teams32Divisions4
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int StartYear { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastChanged { get; set; }

        public LeagueConfiguration Configuration { get; set; }

        [OneToMany]
        public List<Division> Divisions { get; set; }

        [Ignore]
        public List<Division> WesternConference
        {
            get
            {
                return Divisions.OrderByDescending(division => division.Longitude).Take(Divisions.Count / 2).ToList();
            }
        }

        [Ignore]
        public List<Division> EasternConference
        {
            get
            {
                return Divisions.OrderBy(division => division.Longitude).Take(Divisions.Count / 2).ToList();
            }
        }

        [OneToMany]
        public List<Team> Teams { get; set; }

        [OneToMany]
        public List<Season> Seasons { get; set; }

        public League(LeagueConfiguration configuration, int startYear)
        {            
            StartYear = startYear;
            Configuration = configuration;
            Seasons = new List<Season>();
            CreateDivisions();            
        }

        private void CreateDivisions()
        {
            Divisions = new List<Division>();

            var divisionCount = Configuration == LeagueConfiguration.Teams30Divisions6 ? 6 : 4;
            for (var i = 0; i < divisionCount; i++)
            {
                Divisions.Add(new Division());
            }
        }

        public void FillDivisions()
        {
            var maxDivisionTeamCounts = GetDivisionTeamCounts();

            while (!Enumerable.SequenceEqual(Divisions.Select(division => division.Teams.Count).ToArray(), maxDivisionTeamCounts))
            {
                (Team team, Division division) bestPair = (null, null);
                var furthestNeighbour = double.MinValue;

                for (var i = 0; i < Teams.Count; i++)
                {
                    var team = Teams[i];
                    (Team team, Division division) pair = (null, null);

                    if (Divisions.Any(division => division.Teams.Contains(team)))
                    {
                        continue;
                    }

                    var closestDistance = double.MaxValue;                    

                    for (var j = 0; j < Divisions.Count; j++)
                    {
                        var division = Divisions[j];
                        var divisionHasRoom = division.Teams.Count < maxDivisionTeamCounts[j];
                        var squaredDistance = division.SquaredDistanceTo(team.City);

                        if (divisionHasRoom && squaredDistance < closestDistance)
                        {
                            closestDistance = squaredDistance;
                            pair = (team, division);
                        }
                    }

                    if (closestDistance > furthestNeighbour)
                    {
                        furthestNeighbour = closestDistance;
                        bestPair = pair;
                    }
                }

                bestPair.division.Teams.Add(bestPair.team);
                Divisions = Divisions.OrderBy(division => division.Teams.Count).ToList();
            }
        }

        private int[] GetDivisionTeamCounts()
        {
            switch (Configuration)
            {
                case LeagueConfiguration.Teams30Divisions4:
                    return new int[] { 7, 7, 8, 8 };                    
                case LeagueConfiguration.Teams30Divisions6:
                    return new int[] { 5, 5, 5, 5, 5, 5 };
                case LeagueConfiguration.Teams31Divisions4:
                    return new int[] { 7, 8, 8, 8 };
                default:
                    return new int[] { 8, 8, 8, 8 };
            }
        }
    }
}
