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

        [ManyToOne("ScorerKey", "Goals")]
        public Player Scorer { get; set; }

        [ManyToOne("FirstAssistKey", "FirstAssists")]
        public Player FirstAssister { get; set; }

        [ManyToOne("SecondAssistKey", "SecondAssists")]
        public Player SecondAssister { get; set; }
    }
}
