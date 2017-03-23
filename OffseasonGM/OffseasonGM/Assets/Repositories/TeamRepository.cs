﻿using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class TeamRepository
    {
        SQLiteConnection connection;
        Random random;
        CityRepository cityRepo;
        NickNameRepository nickNameRepo;
        PlayerRepository playerRepo;

        public TeamRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            random = new Random();
            cityRepo = new CityRepository(dbPath);
            nickNameRepo = new NickNameRepository(dbPath);
            playerRepo = new PlayerRepository(dbPath);
        }

        public List<Team> CreateTeams(int n)
        {
            var cities = cityRepo.GetRandomSelection(n);
            var nickNames = nickNameRepo.GetRandomSelection(n);

            return Enumerable.Range(0, n).Select(num => CreateNewTeam(cities[n], nickNames[n])).ToList();
        }

        private Team CreateNewTeam(City city, NickName nickName)
        {
            var team = new Team()
            {
                City = city,
                CityId = city.Id,
                NickName = nickName,
                NickNameId = nickName.Id,
                Players = new List<Player>(),
                Seasons = new List<Season>(),
                HomeGames = new List<Match>(),
                AwayGames = new List<Match>()
            };

            for (var i = 0; i < 3; i++)
            {
                var goalie = playerRepo.CreatePlayer(18, Player.PlayerPosition.Goalie);
                team.Players.Add(goalie);
            }
            for (var i = 0; i < 8; i++)
            {
                var defenseman = playerRepo.CreatePlayer(18, Player.PlayerPosition.Defenseman);
                team.Players.Add(defenseman);
            }
            for (var i = 0; i < 5; i++)
            {
                var center = playerRepo.CreatePlayer(18, Player.PlayerPosition.Center);
                team.Players.Add(center);
            }
            for (var i = 0; i < 5; i++)
            {
                var leftWing = playerRepo.CreatePlayer(18, Player.PlayerPosition.LeftWing);
                team.Players.Add(leftWing);
            }
            for (var i = 0; i < 5; i++)
            {
                var rightWing = playerRepo.CreatePlayer(18, Player.PlayerPosition.RightWing);
                team.Players.Add(rightWing);
            }

            foreach (var player in team.Players)
            {
                var birthDays = random.Next(13);
                for (var i = 0; i < birthDays; i++)
                {
                    playerRepo.AgePlayer(player);
                }

                player.Team = team;
                player.TeamId = team.Id;
                connection.Update(player);
            }

            try
            {
                connection.Insert(team);
                return team;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Team> GetAllTeams()
        {
            return connection.GetAllWithChildren<Team>();
        }
    }
}
