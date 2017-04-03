using OffseasonGM.Models;
using SQLite;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;


namespace OffseasonGM.Assets.Repositories
{
    public class LastNameRepository
    {
        SQLiteConnection connection;     
        List<Nation> nations;

        public LastNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);            
            nations = connection.GetAllWithChildren<Nation>().ToList();            
            
            var lastNameCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM LastName");

            if (lastNameCount == 0)
            {
                SeedLastNames();
            }
        }

        public LastName AddNewLastName(string name)
        {
            try
            {
                var lastName = new LastName { Name = name, Nations = new List<Nation>() };
                connection.InsertWithChildren(lastName);
                return lastName;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<LastName> GetAllLastNames()
        {
            return connection.GetAllWithChildren<LastName>();
        }

        private void SeedLastNames()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.LastNames.txt");
            string line;
            using (var reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var row = line.Split('|');
                    var name = row[0];
                    var lastName = AddNewLastName(name);

                    var nationNames = row.Skip(1).ToList();
                    foreach (var nationName in nationNames)
                    {
                        var nation = nations.First(n => n.Name == nationName);
                        lastName.Nations.Add(nation);
                        connection.UpdateWithChildren(lastName);
                        nation.LastNames.Add(lastName);
                        connection.UpdateWithChildren(nation);
                    }
                }
            }
        }
    }
}
