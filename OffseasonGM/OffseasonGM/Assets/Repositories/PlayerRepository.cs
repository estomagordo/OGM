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
        Random random;
        FirstNameRepository firstNameRepo;
        LastNameRepository lastNameRepo;
        NationRepository nationRepo;        

        public PlayerRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            firstNameRepo = new FirstNameRepository(dbPath);
            lastNameRepo = new LastNameRepository(dbPath);
            nationRepo = new NationRepository(dbPath);            
            random = new Random();
        }

        public List<Player> GetAllPlayers()
        {
            return connection.GetAllWithChildren<Player>();
        }         

        private Player InsertPlayer(int firstNameId, int lastNameId)
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
        
        private Player InsertPlayer(Player player)
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
    }
}
