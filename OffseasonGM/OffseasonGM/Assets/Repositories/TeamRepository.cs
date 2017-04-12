using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class TeamRepository : IRepository
    {
        SQLiteConnection connection;

        public TeamRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }        

        public void Save(Team team)
        {
            if (team.Id == 0)
            {
                Insert(team);
            }
            else
            {
                Update(team);
            }
        }

        private void Insert(Team team)
        {
           connection.Insert(team);
        }

        private void Update(Team team)
        {
            connection.UpdateWithChildren(team);
        }

        public List<Team> GetAll()
        {
            return connection.GetAllWithChildren<Team>();
        }
    }
}
