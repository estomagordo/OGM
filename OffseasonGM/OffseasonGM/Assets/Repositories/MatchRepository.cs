using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Assets.Repositories
{
    public class MatchRepository
    {
        SQLiteConnection connection;

        public MatchRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }        

        public void UpdateMatch(Match match)
        {
            connection.UpdateWithChildren(match);
        }

        public List<Match> GetAllMatches()
        {
            return connection.GetAllWithChildren<Match>();
        }
    }
}
