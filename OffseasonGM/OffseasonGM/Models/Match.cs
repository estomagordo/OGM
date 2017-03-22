using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        [ManyToOne("HomeTeamKey", "HomeGames")]
        public Team HomeTeam { get; set; }

        [ManyToOne("AwayTeamKey", "AwayGames")]
        public Team AwayTeam { get; set; }

        [ManyToMany(typeof(MatchPlayer))]
        public List<Player> Players { get; set; }
    }
}
