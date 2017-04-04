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
using OffseasonGM.Global;

namespace OffseasonGM.Assets.Repositories
{
    public class CityRepository : IRepository
    {
        SQLiteConnection connection;
        List<City> cities;

        public CityRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            
            var cityCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM City");

            if (cityCount == 0)
            {
                SeedCities();
            }

            cities = GetAllCities();
        }

        public City AddNewCity(string name, double latitude, double longitude)
        {
            try
            {
                var city = new City() { Name = name, Latitude = latitude, Longitude = longitude };
                connection.Insert(city);
                return city;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<City> GetAllCities()
        {
            return connection.Table<City>().ToList();
        }

        public List<City> GetRandomSelection(int n)
        {
            var remaining = cities.Count;
            var selected = new List<City>();

            foreach (var city in cities)
            {
                var needed = (double)n / (double)remaining;
                var result = GlobalObjects.Random.NextDouble();

                if (result <= needed)
                {
                    selected.Add(city);
                    n--;
                }

                remaining--;

                if (n == 0)
                {
                    break;
                }
            }

            return selected;
        }

        private void SeedCities()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.Cities.txt");
            string line;
            using (var reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var row = line.Split('|');
                    var name = row[0];
                    var latitude = double.Parse(row[1]);
                    var longitude = double.Parse(row[2]);
                    AddNewCity(name, latitude, longitude);
                }
            }
        }
    }
}
