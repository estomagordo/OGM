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
    public class NickNameRepository
    {
        SQLiteConnection connection;
        List<NickName> nickNames;

        public NickNameRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);

            var nickNameCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM NickName");

            if (nickNameCount == 0)
            {
                SeedNickNames();
            }

            nickNames = GetAllNickNames();
        }

        public NickName AddNewNickName(string name)
        {
            try
            {
                var nickName = new NickName() { Name = name };
                connection.Insert(nickName);
                return nickName;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<NickName> GetAllNickNames()
        {
            return connection.Table<NickName>().ToList();
        }        

        public List<NickName> GetRandomSelection(int n)
        {
            var remaining = nickNames.Count;
            var selected = new List<NickName>();

            foreach (var nickName in nickNames)
            {
                var needed = (double)n / (double)remaining;
                var result = GlobalObjects.Random.NextDouble();

                if (result <= needed)
                {
                    selected.Add(nickName);
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

        private void SeedNickNames()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("OffseasonGM.Assets.Text.NickNames.txt");
            string line;
            using (var reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var name = line.Trim();
                    AddNewNickName(name);
                }
            }
        }
    }
}
