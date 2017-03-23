﻿using OffseasonGM.Models;
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
        private Dictionary<string, List<FirstName>> _firstNamesPerNationName;
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

        Dictionary<string, List<FirstName>> FirstNamesPerNationName
        {
            get
            {
                if (_firstNamesPerNationName == null)
                {
                    var dictionary = new Dictionary<string, List<FirstName>>();

                    foreach (var nation in nations)
                    {
                        dictionary[nation.Name] = new List<FirstName>();
                    }
                    foreach (var firstName in FirstNames)
                    {
                        foreach (var nation in firstName.Nations)
                        {
                            dictionary[nation.Name].Add(firstName);
                        }
                    }

                    _firstNamesPerNationName = dictionary;
                }

                return _firstNamesPerNationName;
            }
            set
            {
                _firstNamesPerNationName = value;
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

        public List<FirstName> GetAllFirstNamesForNationName(string nationName)
        {
            return FirstNamesPerNationName[nationName];
        }
        
        public FirstName GetRandomFirstNameForNationName(string nationName)
        {
            var nameList = FirstNamesPerNationName[nationName];
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