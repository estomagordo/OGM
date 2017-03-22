using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class TeamRepository
    {
        SQLiteConnection connection;

        public TeamRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public Team AddNewSeason(int cityId, int nickNameId)
        {
            try
            {
                var team = new Team() { CityId = cityId, NickNameId = nickNameId, AwayGames = new List<Match>(), HomeGames = new List<Match>(), Players = new List<Player>(), Seasons = new List<Season>() };
                connection.Insert(team);
                return team;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Team> GetAllTeams()
        {
            return connection.GetAllWithChildren<Team>();
        }
    }
}
