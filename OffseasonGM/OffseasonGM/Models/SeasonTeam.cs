using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace OffseasonGM.Models
{
    public class SeasonTeam
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        [ForeignKey(typeof(Team))]
        public int TeamId { get; set; }        
    }
}
