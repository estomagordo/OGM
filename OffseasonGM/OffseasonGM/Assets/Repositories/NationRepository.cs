﻿using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Repositories
{
    public class NationRepository
    {
        SQLiteConnection connection;

        public NationRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);            
            
            var nationCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Nation");

            if (nationCount == 0)
            {
                SeedNations();
            }            
        }

        public void AddNewNation(string name, string adjective, double frequency)
        {
            try
            {
                connection.Insert(new Nation { Name = name, Adjective = adjective, Frequency = frequency, FirstNames = new List<FirstName>(), LastNames = new List<LastName>() });
            }
            catch (Exception e)
            {
                //TODO: Error handling.
            }
        }

        public List<Nation> GetAllNations()
        {
            return connection.GetAllWithChildren<Nation>().ToList();
        }

        private void SeedNations()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.Nations.txt");
            string line;
            using (var reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var row = line.Split('|');
                    var name = row[0];
                    var adjective = row[1];
                    var frequency = double.Parse(row[2]);
                    AddNewNation(name, adjective, frequency);
                }
            }
        }
    }
}