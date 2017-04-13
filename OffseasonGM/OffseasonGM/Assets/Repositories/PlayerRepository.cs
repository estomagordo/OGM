using OffseasonGM.Models;
using OffseasonGM.Extensions;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class PlayerRepository : IRepository
    {
        SQLiteConnection connection;

        public PlayerRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public void Save(Player player)
        {
            if (player.Id == 0)
            {
                Insert(player);
            }
            else
            {
                Update(player);
            }
        }

        private void Insert(Player player)
        {
            connection.Insert(player);
        }

        private void Update(Player player)
        {
            connection.UpdateWithChildren(player);
        }

        public List<Player> GetAll()
        {
            return connection.GetAllWithChildren<Player>();
        }
    }
}
