﻿using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace OffseasonGM.Models
{
    public class NickName
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [OneToMany]
        public List<Team> Teams { get; set; }
    }
}
