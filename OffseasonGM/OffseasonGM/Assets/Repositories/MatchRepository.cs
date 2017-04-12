using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Assets.Repositories
{
    public class MatchRepository : IRepository
    {
        SQLiteConnection connection;

        public MatchRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public void Save(Match match)
        {
            if (match.Id == 0)
            {
                Insert(match);
            }
            else
            {
                Update(match);
            }
        }

        private void Update(Match match)
        {
            connection.UpdateWithChildren(match);
        }

        private void Insert(Match match)
        {
            connection.Insert(match);
        }

        public List<Match> GetAll()
        {
            return connection.GetAllWithChildren<Match>();
        }
    }
}
