using OffseasonGM.Models;
using SQLite;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Repositories
{
    public class FirstNameRepository
    {
        SQLiteConnection connection;

        public FirstNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            connection.CreateTable<FirstName>();
        }

        public void AddNewFirstName(string name)
        {
            try
            {
                connection.Insert(new FirstName { Name = name });
            }
            catch (Exception e)
            {
                //TODO: Error handling.
            }
        }

        public List<FirstName> GetAllFirstNames()
        {
            return connection.Table<FirstName>().ToList();
        }
    }
}