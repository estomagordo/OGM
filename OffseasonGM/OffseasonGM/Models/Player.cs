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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Team))]
        public int TeamId { get; set; }

        [ManyToMany(typeof(SeasonPlayer))]
        public List<Season> Seasons { get; set; }

        [ManyToMany(typeof(MatchPlayer))]
        public List<Match> Matches { get; set; }

        [ForeignKey(typeof(FirstName))]
        public int FirstNameId { get; set; }

        [ForeignKey(typeof(LastName))]
        public int LastNameId { get; set; }

        [OneToMany("ScorerKey", "Scorer")]
        public List<Goal> Goals { get; set; }

        [OneToMany("FirstAssistKey", "FirstAssister")]
        public List<Goal> FirstAssists { get; set; }

        [OneToMany("SecondAssistKey", "SecondAssister")]
        public List<Goal> SecondAssists { get; set; }
    }
}
