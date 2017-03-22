using SQLiteNetExtensions.Attributes;

namespace OffseasonGM.Models
{
    public class MatchPlayer
    {
        [ForeignKey(typeof(Match))]
        public int MatchId { get; set; }

        [ForeignKey(typeof(Player))]
        public int PlayerId { get; set; }
    }
}
