using SQLiteNetExtensions.Attributes;

namespace OffseasonGM.Models
{
    public class NationLastName
    {
        [ForeignKey(typeof(Nation))]
        public int NationId { get; set; }

        [ForeignKey(typeof(LastName))]
        public int LastNameId { get; set; }
    }
}
