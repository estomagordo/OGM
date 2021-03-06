﻿using OffseasonGM.Extensions;
using OffseasonGM.Global;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Player
    {
        public enum PlayerPosition
        {
            Goalie,
            Defenseman,
            Center,
            LeftWing,
            RightWing
        }

        private const int _meanPeakAge = 28;
        private const int _peakAgeVariance = 4;
        private const int _defensemanLaterPeak = 2;
        private const int _meanRetireAge = 37;
        private const int _retireAgeVariance = 4;
        private const int _defensemanLaterRetirement = 2;

        private const double improveContraDeclineFactor = -2.0;
        private const double _weakStatMeanStart = 10.0;
        private const double _normalStatMeanStart = 15.0;
        private const double _strongStatMeanStart = 20.0;
        private const double _stableStatMean = 2.0;
        private const double _swingyStatMean = 4.0;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Nation))]
        public int NationId { get; set; }

        [ManyToOne]
        public Nation Nation { get; set; }

        [ForeignKey(typeof(Team))]
        public int TeamId { get; set; }

        [ManyToOne]
        public Team Team { get; set; }

        [ManyToMany(typeof(SeasonPlayer))]
        public List<Season> Seasons { get; set; }

        [ManyToMany(typeof(MatchPlayer))]
        public List<Match> Matches { get; set; }

        [ForeignKey(typeof(FirstName))]
        public int FirstNameId { get; set; }

        [ManyToOne]
        public FirstName FirstName { get; set; }

        [ForeignKey(typeof(LastName))]
        public int LastNameId { get; set; }
        
        [ManyToOne]
        public LastName LastName { get; set; }

        [OneToMany("ScorerKey", "Scorer")]
        public List<Goal> Goals { get; set; }

        [OneToMany("FirstAssistKey", "FirstAssister")]
        public List<Goal> FirstAssists { get; set; }

        [OneToMany("SecondAssistKey", "SecondAssister")]
        public List<Goal> SecondAssists { get; set; }

        public PlayerPosition Position { get; set; }

        public int Age { get; set; }
        public int PeakAge { get; set; }
        public int RetireAge { get; set; }

        public double ImproveSpeed { get; set; }
        public double DeclineSpeed { get; set; }

        public double Defense { get; set; }
        public double Endurance { get; set; }
        public double Fitness { get; set; }
        public double Passing { get; set; }
        public double PuckControl { get; set; }
        public double ReboundControl { get; set; }
        public double Saving { get; set; }
        public double Shooting { get; set; }
        public double Skating { get; set; }

        public bool Retired { get; set; }

        [Ignore]
        public double PassingContribution => Position == PlayerPosition.Goalie
            ? Passing * 0.01
            : Position == PlayerPosition.Defenseman
                ? Passing * 0.5
                : Passing;

        [Ignore]
        public double Overall
        {
            get
            {
                switch (Position)
                {
                    case PlayerPosition.Goalie:
                        return (Saving + ReboundControl + Endurance * 0.5) / 2.5;
                    case PlayerPosition.Defenseman:
                        return (Defense + Skating + Shooting * 0.5 + Endurance * 0.5 + Passing * 0.5 + PuckControl * 0.5) / 4.0;
                    case PlayerPosition.Center:
                        return (Passing + Shooting + PuckControl + Skating * 0.75 + Defense * 0.5 + Endurance * 0.5) / 4.75;
                    default:
                        return (Skating + Shooting + Passing + PuckControl * 0.75 + Defense * 0.5 + Endurance * 0.5) / 4.75;
                }
            }
        }

        [Ignore]
        public string ShortPosition
        {
            get
            {
                switch (Position)
                {
                    case PlayerPosition.Goalie:
                        return Assets.Resources.Default.G;                        
                    case PlayerPosition.Defenseman:
                        return Assets.Resources.Default.D;                        
                    case PlayerPosition.Center:
                        return Assets.Resources.Default.C;
                    case PlayerPosition.LeftWing:
                        return Assets.Resources.Default.LW;
                    default:
                        return Assets.Resources.Default.RW;
                }
            }
        }

        [Ignore]
        public List<(Season season, int matchesPlayed, int goalCount, int assistCount, int pointCount)> SkaterSeasonStats
        {
            get
            {
                var seasonStats = new List<(Season, int, int, int, int)>();

                foreach (var season in Seasons)
                {
                    var matchCount = Matches.Count(match => match.SeasonId == season.Id);
                    var goalCount = Goals.Count(goal => goal.SeasonId == season.Id);
                    var assistCount = FirstAssists.Count(assist => assist.SeasonId == season.Id) + SecondAssists.Count(assist => assist.SeasonId == season.Id);
                    var pointCount = goalCount + assistCount;

                    seasonStats.Add((season, matchCount, goalCount, assistCount, pointCount));
                }

                return seasonStats;
            }
        }

        [Ignore]
        public (int matchesPlayed, int goalCount, int assistCount, int pointCount) SkaterCareerStats
        {
            get
            {
                var matchCount = SkaterSeasonStats.Sum(season => season.matchesPlayed);
                var goalCount = SkaterSeasonStats.Sum(season => season.goalCount);
                var assistCount = SkaterSeasonStats.Sum(season => season.assistCount);
                var pointCount = goalCount + assistCount;

                return (matchCount, goalCount, assistCount, pointCount);
            }
        }

        [Ignore]
        public List<(Season season, int matchesPlayed, double savePercentage, double gaa, int shutouts)> GoalieSeasonStats
        {
            get
            {
                var seasonStats = new List<(Season, int, double, double, int)>();

                foreach (var season in Seasons)
                {
                    var matchesPlayed = Matches.Where(match => match.SeasonId == season.Id);

                    var matchCount = matchesPlayed.Count();
                    var concededCount = matchesPlayed.Sum(match => match.ConcededForGoalie(this));
                    var shotsAgainstCount = matchesPlayed.Sum(match => match.ShotsAgainstForGoalie(this));

                    var savePercentage = 100.0 * (1.0 - ((double)concededCount / (double)shotsAgainstCount));
                    var gaa = (double)concededCount / (double)matchCount;
                    var shutOuts = matchesPlayed.Count(match => match.ConcededForGoalie(this) == 0);

                    seasonStats.Add((season, matchCount, savePercentage, gaa, shutOuts));
                }

                return seasonStats;
            }
        }

        [Ignore]
        public (int matchesPlayed, double savePercentage, double gaa, int shutouts) GoalieCareerStats
        {
            get
            {
                var matchCount = Matches.Count();
                var concededCount = Matches.Sum(match => match.ConcededForGoalie(this));
                var shotsAgainstCount = Matches.Sum(match => match.ShotsAgainstForGoalie(this));

                var savePercentage = 100.0 * (1.0 - ((double)concededCount / (double)shotsAgainstCount));
                var gaa = (double)concededCount / (double)matchCount;
                var shutOuts = Matches.Count(match => match.ConcededForGoalie(this) == 0);

                return (matchCount, savePercentage, gaa, shutOuts);
            }
        }

        public Player(int age, PlayerPosition position, Nation nation, FirstName firstName, LastName lastName)
        {
            Nation = nation;
            NationId = nation.Id;
            FirstName = firstName;
            FirstNameId = firstName.Id;
            LastName = lastName;
            LastNameId = lastName.Id;
            Position = position;
            Age = age;
            Seasons = new List<Season>();
            Matches = new List<Match>();
            Goals = new List<Goal>();
            FirstAssists = new List<Goal>();
            SecondAssists = new List<Goal>();
            Retired = false;
            
            PeakAge = _meanPeakAge + (int)(_peakAgeVariance * GlobalObjects.Random.NextDouble()) + (position == Player.PlayerPosition.Defenseman ? _defensemanLaterPeak : 0);
            RetireAge = _meanRetireAge + (int)(_retireAgeVariance * GlobalObjects.Random.NextDouble()) + (position == Player.PlayerPosition.Defenseman ? _defensemanLaterRetirement : 0);

            ImproveSpeed = 0.5 + 0.5 * GlobalObjects.Random.NextDouble();
            DeclineSpeed = (0.5 + 0.5 * GlobalObjects.Random.NextDouble()) * improveContraDeclineFactor;

            Defense = Math.Max(0.0,
                (position == Player.PlayerPosition.Defenseman
                    ? _strongStatMeanStart
                    : position == Player.PlayerPosition.Center
                        ? _normalStatMeanStart
                        : _weakStatMeanStart)
                +
                (position == Player.PlayerPosition.Defenseman
                ? _stableStatMean
                : _swingyStatMean) * GlobalObjects.Random.NextGaussian());

            Endurance = Math.Max(0.0, _normalStatMeanStart + _stableStatMean * GlobalObjects.Random.NextGaussian());

            Fitness = Math.Max(0.0, _normalStatMeanStart + _stableStatMean * GlobalObjects.Random.NextGaussian());

            Passing = Math.Max(0.0,
                (position == Player.PlayerPosition.Defenseman
                    ? _normalStatMeanStart
                    : _strongStatMeanStart)
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * GlobalObjects.Random.NextGaussian());

            PuckControl = Math.Max(0.0, (
                position == Player.PlayerPosition.Defenseman
                    ? _normalStatMeanStart
                    : _strongStatMeanStart)
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * GlobalObjects.Random.NextGaussian());

            ReboundControl = Math.Max(0.0, _strongStatMeanStart + _stableStatMean * GlobalObjects.Random.NextGaussian());

            Saving = Math.Max(0.0, _strongStatMeanStart + _stableStatMean * GlobalObjects.Random.NextGaussian());

            Shooting = Math.Max(0.0,
                (position == Player.PlayerPosition.Defenseman
                    ? _normalStatMeanStart
                    : _strongStatMeanStart)
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * GlobalObjects.Random.NextGaussian());

            Skating = Math.Max(0.0, (
                (position == Player.PlayerPosition.LeftWing || position == Player.PlayerPosition.RightWing)
                    ? _strongStatMeanStart
                    : _normalStatMeanStart)
                +
                _stableStatMean * GlobalObjects.Random.NextGaussian());
        }

        public void HaveBirthday()
        {
            var baseChangeFactor = Age >= PeakAge
                ? DeclineSpeed
                : ImproveSpeed;

            Age++;

            Defense += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Endurance += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Fitness += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Passing += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            PuckControl += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            ReboundControl += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Saving += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Shooting += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;
            Skating += baseChangeFactor * GlobalObjects.Random.NextDouble() * 2.0;

            if (Age == RetireAge)
            {
                Retired = true;
            }
        }

        public override string ToString()
        {
            return FirstName.Name + " " + LastName.Name;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player player)
        {
            return player != null && player.Id == Id;
        }
    }
}
