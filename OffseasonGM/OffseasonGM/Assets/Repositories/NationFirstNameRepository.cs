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
    public class NationFirstNameRepository
    {
        SQLiteConnection connection;

        public NationFirstNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            connection.CreateTable<NationFirstName>();            
        }
    }
}
