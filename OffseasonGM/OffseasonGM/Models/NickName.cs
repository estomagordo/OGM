using SQLite.Net.Attributes;

namespace OffseasonGM.Models
{
    public class NickName
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
