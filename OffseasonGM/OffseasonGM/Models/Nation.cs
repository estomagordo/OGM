﻿using SQLite;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace OffseasonGM.Models
{    
    public class Nation
    {
        private const string flagUriFormat = "OffseasonGM.Assets.Images.{0}_flat.png";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
                
        public string Name { get; set; }

        public string Adjective { get; set; }

        public double Frequency { get; set; }

        [ManyToMany(typeof(NationFirstName))]
        public List<FirstName> FirstNames { get; set; }

        [ManyToMany(typeof(NationLastName))]
        public List<LastName> LastNames { get; set; }

        [OneToMany]
        public List<Player> Players { get; set; }

        public string FlagUri
        {
            get
            {                
                return string.Format(flagUriFormat, Name.ToLower());                
            }
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Nation);
        }

        public bool Equals(Nation nation)
        {
            return nation != null && nation.Id == Id;
        }
    }
}
