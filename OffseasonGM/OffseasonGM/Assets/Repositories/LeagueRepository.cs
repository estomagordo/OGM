using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class LeagueRepository
    {
        SQLiteConnection connection;

        public LeagueRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public League AddNewLeague()
        {
            try
            {
                var timeStamp = DateTime.Now;
                var league = new League() { Created = timeStamp, LastChanged = timeStamp, Teams = new List<Team>() };
                connection.Insert(league);
                return league;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<League> GetAllLeagues()
        {
            return connection.GetAllWithChildren<League>();
        }
    }
}
