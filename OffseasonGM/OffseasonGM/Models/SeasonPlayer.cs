using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace OffseasonGM.Models
{
    public class SeasonPlayer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        [ForeignKey(typeof(Player))]
        public int PlayerId { get; set; }
    }
}
