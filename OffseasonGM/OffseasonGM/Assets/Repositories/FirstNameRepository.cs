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
    public class FirstNameRepository : IRepository
    {
        SQLiteConnection connection;
        List<Nation> nations;   

        public FirstNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            nations = connection.GetAllWithChildren<Nation>().ToList();            
            
            var firstNameCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM FirstName");

            if (firstNameCount == 0)
            {
                SeedFirstNames();
            }
        }

        public FirstName AddNewFirstName(string name)
        {
            try
            {
                var firstName = new FirstName { Name = name, Nations = new List<Nation>() };
                connection.InsertWithChildren(firstName);
                return firstName;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<FirstName> GetAllFirstNames()
        {
            return connection.GetAllWithChildren<FirstName>();
        }

        private void SeedFirstNames()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.FirstNames.txt");
            string line;
            using (var reader = new StreamReader(stream, Encoding.GetEncoding("Windows-1252")))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var row = line.Split('|');
                    var name = row[0];                    
                    var firstName = AddNewFirstName(name);

                    var nationNames = row.Skip(1).ToList();
                    foreach (var nationName in nationNames)
                    {
                        var nation = nations.First(n => n.Name == nationName);
                        firstName.Nations.Add(nation);
                        connection.UpdateWithChildren(firstName);
                        nation.FirstNames.Add(firstName);
                        connection.UpdateWithChildren(nation);
                    }
                }
            }
        }
    }
}