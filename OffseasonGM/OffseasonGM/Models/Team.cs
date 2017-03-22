using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace OffseasonGM.Models
{
    public class Team
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(City))]
        public int CityId { get; set; }

        [ForeignKey(typeof(NickName))]
        public int NickNameId { get; set; }

        [OneToMany]
        public List<Player> Players { get; set; }

        [ManyToMany(typeof(SeasonTeam))]
        public List<Season> Seasons { get; set; }

        [OneToMany("HomeTeamKey", "HomeTeam")]
        public List<Match> HomeGames { get; set; }

        [OneToMany("AwayTeamKey", "AwayTeam")]
        public List<Match> AwayGames { get; set; }
    }
}
