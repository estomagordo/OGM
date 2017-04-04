using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class SeasonRepository : IRepository
    {
        SQLiteConnection connection;

        public SeasonRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }        

        public void UpdateSeason(Season season)
        {
            connection.UpdateWithChildren(season);
        }

        public List<Season> GetAllSeasons()
        {
            return connection.GetAllWithChildren<Season>();
        }
    }
}
