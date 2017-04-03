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
            connection.CreateTable<City>();
            connection.CreateTable<FirstName>();
            connection.CreateTable<Goal>();
            connection.CreateTable<LastName>();
            connection.CreateTable<League>();
            connection.CreateTable<Match>();
            connection.CreateTable<Nation>();
            connection.CreateTable<NickName>();
            connection.CreateTable<Player>();
            connection.CreateTable<Season>();
            connection.CreateTable<Team>();
            connection.CreateTable<MatchPlayer>();
            connection.CreateTable<NationFirstName>();
            connection.CreateTable<NationLastName>();
            connection.CreateTable<SeasonPlayer>();
            connection.CreateTable<SeasonTeam>();
            connection.CreateTable<Division>();
        }

        private void DeleteAllContent()
        {
            connection.DeleteAll<City>();
            connection.DeleteAll<FirstName>();
            connection.DeleteAll<Goal>();
            connection.DeleteAll<LastName>();
            connection.DeleteAll<League>();
            connection.DeleteAll<Match>();
            connection.DeleteAll<Nation>();
            connection.DeleteAll<NickName>();
            connection.DeleteAll<Player>();
            connection.DeleteAll<Season>();
            connection.DeleteAll<Team>();
            connection.DeleteAll<MatchPlayer>();
            connection.DeleteAll<NationFirstName>();
            connection.DeleteAll<NationLastName>();
            connection.DeleteAll<SeasonPlayer>();
            connection.DeleteAll<SeasonTeam>();
            connection.DeleteAll<Division>();
        }
    }
}
