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

        [OneToMany("HomeTeamKey", "HomeTeam")]
        public List<Match> HomeGames { get; set; }

        [OneToMany("AwayTeamKey", "AwayTeam")]
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

        public void ArrangeBestTeam()
        {
            GoalieOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Goalie).OrderByDescending(player => player.Overall).ToList();
            DefenseManOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Defenseman).OrderByDescending(player => player.Overall).ToList();
            CenterOrdering = Players.Where(player => player.Position == Player.PlayerPosition.Center).OrderByDescending(player => player.Overall).ToList();
            LeftWingOrdering = Players.Where(player => player.Position == Player.PlayerPosition.LeftWing).OrderByDescending(player => player.Overall).ToList();
            RightWingOrdering = Players.Where(player => player.Position == Player.PlayerPosition.RightWing).OrderByDescending(player => player.Overall).ToList();
        }
    }
}
