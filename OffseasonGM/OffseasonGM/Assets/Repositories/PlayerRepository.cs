using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class PlayerRepository
    {
        SQLiteConnection connection;

        public PlayerRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public Player AddNewPlayer(int firstNameId, int lastNameId)
        {
            try
            {                
                var player = new Player() { FirstNameId = firstNameId, LastNameId = lastNameId, Seasons = new List<Season>(), Matches = new List<Match>() };
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
