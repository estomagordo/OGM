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
    public class FirstNameRepository
    {
        private Dictionary<Nation, List<FirstName>> _firstNamesPerNation;
        private List<FirstName> _firstNames;

        SQLiteConnection connection;
        Random random;
        List<Nation> nations;
        
        List<FirstName> FirstNames
        {
            get
            {
                return _firstNames ?? (_firstNames = connection.GetAllWithChildren<FirstName>().ToList());
            }
            set
            {
                _firstNames = value;
            }
        }

        Dictionary<Nation, List<FirstName>> FirstNamesPerNation
        {
            get
            {
                if (_firstNamesPerNation == null)
                {
                    var dictionary = new Dictionary<Nation, List<FirstName>>();

                    foreach (var nation in nations)
                    {
                        dictionary[nation] = new List<FirstName>();
                    }
                    foreach (var firstName in FirstNames)
                    {
                        foreach (var nation in firstName.Nations)
                        {
                            dictionary[nation].Add(firstName);
                        }
                    }

                    _firstNamesPerNation = dictionary;
                }

                return _firstNamesPerNation;
            }
            set
            {
                _firstNamesPerNation = value;
            }
        }

        public FirstNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            random = new Random();
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
            return FirstNames;
        }

        public List<FirstName> GetAllFirstNamesForNation(Nation nation)
        {
            return FirstNamesPerNation[nation];
        }
        
        public FirstName GetRandomFirstNameForNation(Nation nation)
        {
            var nameList = FirstNamesPerNation[nation];
            return nameList[random.Next(nameList.Count)];
        }

        private void SeedFirstNames()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.FirstNames.txt");
            string line;
            using (var reader = new StreamReader(stream))
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