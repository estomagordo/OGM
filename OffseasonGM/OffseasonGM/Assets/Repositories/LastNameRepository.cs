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
        private Dictionary<Nation, List<LastName>> _lastNamesPerNation;        
        private List<LastName> _lastNames;

        SQLiteConnection connection;
        Random random;
        List<Nation> nations;

        List<LastName> LastNames
        {
            get
            {
                return _lastNames ?? (_lastNames = connection.GetAllWithChildren<LastName>().ToList());
            }
            set
            {
                _lastNames = value;
            }
        }

        Dictionary<Nation, List<LastName>> LastNamesPerNation
        {
            get
            {
                if (_lastNamesPerNation == null)
                {
                    var dictionary = new Dictionary<Nation, List<LastName>>();

                    foreach (var nation in nations)
                    {
                        dictionary[nation] = new List<LastName>();
                    }
                    foreach (var lastName in LastNames)
                    {
                        foreach (var nation in lastName.Nations)
                        {
                            dictionary[nation].Add(lastName);
                        }
                    }

                    _lastNamesPerNation = dictionary;
                }

                return _lastNamesPerNation;
            }
            set
            {
                _lastNamesPerNation = value;
            }
        }

        public LastNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            random = new Random();
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
            return LastNames;
        }

        public List<LastName> GetAllLastNamesForNation(Nation nation)
        {
            return LastNamesPerNation[nation];
        }

        public LastName GetRandomLastNameForNation(Nation nation)
        {
            var nameList = LastNamesPerNation[nation];
            return nameList[random.Next(nameList.Count)];
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
