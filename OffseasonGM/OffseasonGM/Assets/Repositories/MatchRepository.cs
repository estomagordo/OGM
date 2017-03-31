using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Assets.Repositories
{
    public class MatchRepository
    {
        SQLiteConnection connection;

        public MatchRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public Match AddNewMatch(int seasonId, Team home, Team away)
        {
            try
            {
                var players = home.Players.Concat(away.Players).ToList();
                var match = new Match() { SeasonId = seasonId, HomeTeam = home, AwayTeam = away, Players = players };
                connection.InsertWithChildren(match);
                return match;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateMatch(Match match)
        {
            connection.UpdateWithChildren(match);
        }

        public List<Match> GetAllMatches()
        {
            return connection.GetAllWithChildren<Match>();
        }
    }
}
