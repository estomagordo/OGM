using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Models
{
    public class Team
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(City))]
        public int CityId { get; set; }

        [ManyToOne]
        public City City { get; set; }

        [ForeignKey(typeof(NickName))]
        public int NickNameId { get; set; }

        [ManyToOne]
        public NickName NickName { get; set; }

        public double FirstGoalieShare { get; set; }
        public int FirstPairTime { get; set; }
        public int SecondPairTime { get; set; }
        public int ThirdPairTime { get; set; }
        public int FirstLineTime { get; set; }
        public int SecondLineTime { get; set; }
        public int ThirdLineTime { get; set; }
        public int FourthLineTime { get; set; }



        [OneToMany]
        public List<Player> Players { get; set; }

        [ManyToMany(typeof(SeasonTeam))]
        public List<Season> Seasons { get; set; }

        [OneToMany("HomeTeamKey", "HomeTeam"), ForeignKey(typeof(Match))]
        public List<Match> HomeGames { get; set; }

        [OneToMany("AwayTeamKey", "AwayTeam"), ForeignKey(typeof(Match))]
        public List<Match> AwayGames { get; set; }

        [Ignore]
        public List<Player> GoalieOrdering { get; set; }
        [Ignore]
        public List<Player> DefenseManOrdering { get; set; }
        [Ignore]
        public List<Player> CenterOrdering { get; set; }
        [Ignore]
        public List<Player> LeftWingOrdering { get; set; }
        [Ignore]
        public List<Player> RightWingOrdering { get; set; }

        [Ignore]
        public List<Player> Lineup
        {
            get
            {
                return  GoalieOrdering.Take(2).Concat(
                        DefenseManOrdering.Take(6).Concat(
                        CenterOrdering.Take(4).Concat(
                        LeftWingOrdering.Take(4).Concat(
                        RightWingOrdering.Take(4)))))
                        .ToList();
            }
        }

        [Ignore]
        public Season LatestSeason
        {
            get
            {
                return Seasons.Last();
            }
        }
        [Ignore]
        public IEnumerable<Match> LatestSeasonHomeGames
        {
            get
            {
                return HomeGames.Where(game => game.SeasonId == LatestSeason.Id);
            }
        }
        [Ignore]
        public IEnumerable<Match> LatestSeasonAwayGames
        {
            get
            {
                return AwayGames.Where(game => game.SeasonId == LatestSeason.Id);
            }
        }
        [Ignore]
        public IEnumerable<Match> LatestSeasonMatches
        {
            get
            {
                return LatestSeasonHomeGames.Concat(LatestSeasonAwayGames);
            }
        }
        [Ignore]
        public int LatestSeasonMatchCount
        {
            get
            {
                return LatestSeasonMatches.ToList().Count;
            }
        }
        [Ignore]
        public int LatestSeasonWinCount
        {
            get
            {
                return LatestSeasonHomeGames.Count(match => match.HomeGoalCount > match.AwayGoalCount) + LatestSeasonAwayGames.Count(match => match.AwayGoalCount > match.HomeGoalCount);
            }
        }
        [Ignore]
        public int LatestSeasonLossCount
        {
            get
            {
                return LatestSeasonHomeGames.Count(match => match.HomeGoalCount < match.AwayGoalCount) + LatestSeasonAwayGames.Count(match => match.AwayGoalCount < match.HomeGoalCount);
            }
        }
        [Ignore]
        public int LatestSeasonDrawCount
        {
            get
            {
                return LatestSeasonHomeGames.Count(match => match.HomeGoalCount == match.AwayGoalCount) + LatestSeasonAwayGames.Count(match => match.AwayGoalCount == match.HomeGoalCount);
            }
        }
        [Ignore]
        public string FormatedSeasonRecord
        {
            get
            {
                return string.Join(" - ", new[] { LatestSeasonMatchCount, LatestSeasonWinCount, LatestSeasonDrawCount, LatestSeasonLossCount });
            }            
        }

        public List<int> DefenseShiftTimes
        {
            get
            {
                return new List<int> { FirstPairTime, SecondPairTime, ThirdPairTime };
            }
        }

        public List<int> OffenseShiftTimes
        {
            get
            {
                return new List<int> { FirstLineTime, SecondLineTime, ThirdLineTime, FourthLineTime };
            }
        }        

        public Team (City city, NickName nickName)
        {
            City = city;
            CityId = city.Id;
            NickName = nickName;
            NickNameId = nickName.Id;
            FirstGoalieShare = 0.75;
            FirstPairTime = 50;
            SecondPairTime = 45;
            ThirdPairTime = 35;
            FirstLineTime = 40;
            SecondLineTime = 35;
            ThirdLineTime = 30;
            FourthLineTime = 20;
            Players = new List<Player>();
            Seasons = new List<Season>();
            HomeGames = new List<Match>();
            AwayGames = new List<Match>();
        }

        public void ArrangeBestTeam()
        {
            GoalieOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Goalie).OrderByDescending(player => player.Overall).ToList();
            DefenseManOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Defenseman).OrderByDescending(player => player.Overall).ToList();
            CenterOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Center).OrderByDescending(player => player.Overall).ToList();
            LeftWingOrdering = Players.Where(player => player.Position == Player.PlayerPosition.LeftWing).OrderByDescending(player => player.Overall).ToList();
            RightWingOrdering = Players.Where(player => player.Position == Player.PlayerPosition.RightWing).OrderByDescending(player => player.Overall).ToList();
        }        

        public override string ToString()
        {
            return City.Name + " " + NickName.Name;
        }
    }
}
