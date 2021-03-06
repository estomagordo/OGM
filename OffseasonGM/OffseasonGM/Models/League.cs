﻿using OffseasonGM.Extensions;
using OffseasonGM.Global;
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

        private int[] _maxDivisionTeamCounts;        

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

        public Season CurrentSeason
        {
            get
            {
                return Seasons.Last();
            }
        }

        public League(LeagueConfiguration configuration, int startYear)
        {            
            StartYear = startYear;
            Configuration = configuration;
            Seasons = new List<Season>();
            SetDivisionTeamCounts();
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
            var latitudalTeams = Teams.OrderBy(team => team.City.Latitude).ToList();
            PutTeamsInTheCorners(latitudalTeams);

            while (!Enumerable.SequenceEqual(Divisions.Select(division => division.Teams.Count).ToArray(), _maxDivisionTeamCounts))
            {
                (Team team, Division division) bestPair = (null, null);
                var furthestNeighbour = double.MinValue;

                for (var i = 0; i < latitudalTeams.Count; i++)
                {
                    var team = latitudalTeams[i];
                    (Team team, Division division) pair = (null, null);

                    if (Divisions.Any(division => division.Teams.Contains(team)))
                    {
                        continue;
                    }

                    var closestDistance = double.MaxValue;                    

                    for (var j = 0; j < Divisions.Count; j++)
                    {
                        var division = Divisions[j];
                        var divisionHasRoom = division.Teams.Count < _maxDivisionTeamCounts[j];
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

        private void PutTeamsInTheCorners(List<Team> latitudalTeams)
        {
            var east = latitudalTeams.Take(latitudalTeams.Count / 2);
            var west = latitudalTeams.Skip(latitudalTeams.Count / 2);

            var eastLongitudal = east.OrderBy(team => team.City.Longitude).ToList();
            var westLongitudal = west.OrderBy(team => team.City.Longitude).ToList();

            Divisions[0].Teams.Add(eastLongitudal.First());
            Divisions[1].Teams.Add(eastLongitudal.Last());
            Divisions[2].Teams.Add(westLongitudal.First());
            Divisions[3].Teams.Add(westLongitudal.Last());

            if (Divisions.Count > 4)
            {
                Divisions[5].Teams.Add(eastLongitudal[eastLongitudal.Count() / 2]);
                Divisions[6].Teams.Add(westLongitudal[westLongitudal.Count() / 2]);
            }
        }

        private void SetDivisionTeamCounts()
        {
            _maxDivisionTeamCounts = Configuration == LeagueConfiguration.Teams30Divisions4
                ? new int[] { 7, 7, 8, 8 }
                : Configuration == LeagueConfiguration.Teams30Divisions6
                    ? new int[] { 5, 5, 5, 5, 5 }
                    : Configuration == LeagueConfiguration.Teams31Divisions4
                        ? new int[] { 7, 8, 8, 8 }
                        : new int[] { 8, 8, 8, 8 };
        }        
    }
}
