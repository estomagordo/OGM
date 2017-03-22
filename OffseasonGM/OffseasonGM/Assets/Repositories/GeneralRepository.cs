using OffseasonGM.Models;
using SQLite.Net;

namespace OffseasonGM.Assets.Repositories
{
    public class GeneralRepository
    {
        SQLiteConnection connection;

        public GeneralRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            CreateAllTables();
            DeleteAllContent(); // Temporary
        }

        private void CreateAllTables()
        {
            connection.CreateTable<Nation>();
            connection.CreateTable<City>();
            connection.CreateTable<FirstName>();
            connection.CreateTable<LastName>();            
            connection.CreateTable<NickName>();
            connection.CreateTable<NationFirstName>();
            connection.CreateTable<NationLastName>();
        }

        private void DeleteAllContent()
        {
            connection.DeleteAll<Nation>();
            connection.DeleteAll<City>();
            connection.DeleteAll<FirstName>();
            connection.DeleteAll<LastName>();
            connection.DeleteAll<NickName>();
            connection.DeleteAll<NationFirstName>();
            connection.DeleteAll<NationLastName>();
        }
    }
}
