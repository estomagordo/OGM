using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
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

        public void UpdateTeam(Team team)
        {
            connection.UpdateWithChildren(team);
        }

        public List<Team> GetAllTeams()
        {
            return connection.GetAllWithChildren<Team>();
        }
    }
}
