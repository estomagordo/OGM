using SQLite;
using SQLiteNetExtensions.Attributes;

namespace OffseasonGM.Models
{
    public class NationFirstName
    {
        [ForeignKey(typeof(Nation))]
        public int NationId { get; set; }

        [ForeignKey(typeof(FirstName))]
        public int FirstNameId { get; set; }
    }
}
