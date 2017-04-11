using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Match))]
        public int MatchId { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        [ManyToOne("ScorerKey", "Goals")]
        public Player Scorer { get; set; }

        [ManyToOne("FirstAssistKey", "FirstAssists")]
        public Player FirstAssister { get; set; }

        [ManyToOne("SecondAssistKey", "SecondAssists")]
        public Player SecondAssister { get; set; }

        [ManyToOne]
        public Team Team { get; set; }

        public int Period { get; set; }

        public int Seconds { get; set; }

        public Goal()
        {

        }

        public Goal(int seasonId, int matchId, Team team, int period, int seconds, Player scorer, List<Player> assisters)
        {
            SeasonId = seasonId;
            MatchId = matchId;
            Team = team;            
            Period = period;
            Seconds = seconds;
            Scorer = scorer;

            if (assisters.Count > 0)
            {
                FirstAssister = assisters.First();
            }
            if (assisters.Count == 2)
            {
                SecondAssister = assisters.Last();
            }
        }
    }
}
