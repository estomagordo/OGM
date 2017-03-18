using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace OffseasonGM.Models
{
    public class FirstName
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [ManyToMany(typeof(NationFirstName))]
        public List<Nation> Nations { get; set; }
    }
}
