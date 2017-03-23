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
        
        public override string ToString()
        {
            return FirstName.Name + " " + LastName.Name;
        }
    }
}
