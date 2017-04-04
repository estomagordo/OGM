using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Repositories
{
    public class DivisionRepository : IRepository
    {
        SQLiteConnection connection;

        public DivisionRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public void UpdateDivision(Division division)
        {
            connection.UpdateWithChildren(division);
        }

        public List<Division> GetAllDivisions()
        {
            return connection.GetAllWithChildren<Division>();
        }
    }
}
