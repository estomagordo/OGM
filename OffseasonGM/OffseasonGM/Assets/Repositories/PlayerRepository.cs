using OffseasonGM.Models;
using OffseasonGM.Extensions;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class PlayerRepository
    {
        SQLiteConnection connection;
        FirstNameRepository firstNameRepo;
        LastNameRepository lastNameRepo;
        NationRepository nationRepo;        

        public PlayerRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            firstNameRepo = new FirstNameRepository(dbPath);
            lastNameRepo = new LastNameRepository(dbPath);
            nationRepo = new NationRepository(dbPath);
        }

        public Player InsertPlayer(Player player)
        {
            try
            {
                connection.Insert(player);
                return player;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Player> GetAllPlayers()
        {
            return connection.GetAllWithChildren<Player>();
        }
    }
}
