using SQLite;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace OffseasonGM.Models
{    
    public class Nation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
                
        public string Name { get; set; }

        public string Adjective { get; set; }

        public double Frequency { get; set; }

        [ManyToMany(typeof(NationFirstName))]
        public List<FirstName> FirstNames { get; set; }

        [ManyToMany(typeof(NationLastName))]
        public List<LastName> LastNames { get; set; }
    }
}
