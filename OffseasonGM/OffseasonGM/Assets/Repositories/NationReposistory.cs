using OffseasonGM.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Repositories
{
    public class NationReposistory
    {
        SQLiteConnection connection;

        public NationReposistory(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            var entriesCreatedCount = connection.CreateTable<Nation>();
            if (entriesCreatedCount < 1)
            {
                SeedNations();
            }
        }

        public void AddNewNation(string name, string adjective, double frequency)
        {
            try
            {
                connection.Insert(new Nation { Name = name, Adjective = adjective, Frequency = frequency });
            }
            catch (Exception e)
            {
                //TODO: Error handling.
            }
        }

        public List<Nation> GetAllNations()
        {
            return connection.Table<Nation>().ToList();
        }

        private void SeedNations()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Nations.txt");
            using (var reader = new StreamReader(stream))
            {
                var line = reader.ReadLine().Split('|');
                var name = line[0];
                var adjective = line[1];
                var frequency = double.Parse(line[2]);
                AddNewNation(name, adjective, frequency);
            }
        }
    }
}
