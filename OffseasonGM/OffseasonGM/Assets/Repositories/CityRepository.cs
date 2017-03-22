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
    public class CityRepository
    {
        SQLiteConnection connection;

        public CityRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);            
            
            var cityCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM City");

            if (cityCount == 0)
            {
                SeedCities();
            }
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
