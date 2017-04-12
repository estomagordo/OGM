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

        public void Save(Season season)
        {
            if (season.Id == 0)
            {
                Insert(season);
            }
            else
            {
                Update(season);
            }
        }

        private void Update(Season season)
        {
            connection.UpdateWithChildren(season);
        }

        private void Insert(Season season)
        {
            connection.Insert(season);
        }

        public List<Season> GetAll()
        {
            return connection.GetAllWithChildren<Season>();
        }
    }
}
