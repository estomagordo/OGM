using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class SeasonRepository
    {
        SQLiteConnection connection;

        public SeasonRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public Season AddNewSeason(int startYear)
        {
            try
            {
                var season = new Season() { StartYear = startYear, Matches = new List<Match>(), Players = new List<Player>(), Teams = new List<Team>() };
                connection.Insert(season);
                return season;
            }
            catch (Exception ex)
            {
                return null;
            }
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
