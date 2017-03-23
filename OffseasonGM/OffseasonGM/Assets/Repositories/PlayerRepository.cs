using OffseasonGM.Models;
using OffseasonGM.Extensions;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OffseasonGM.Assets.Repositories
{
    public class PlayerRepository
    {
        private const int _meanPeakAge = 28;
        private const int _peakAgeVariance = 4;
        private const int _defensemanLaterPeak = 2;
        private const int _meanRetireAge = 37;
        private const int _retireAgeVariance = 4;
        private const int _defensemanLaterRetirement = 2;

        private const double improveContraDeclineFactor = 2.0;
        private const double _weakStatMeanStart = 10.0;
        private const double _normalStatMeanStart = 15.0;
        private const double _strongStatMeanStart = 20.0;
        private const double _stableStatMean = 2.0;
        private const double _swingyStatMean = 4.0;

        SQLiteConnection connection;
        Random random;
        FirstNameRepository firstNameRepo;
        LastNameRepository lastNameRepo;
        NationRepository nationRepo;        

        public PlayerRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
            firstNameRepo = new FirstNameRepository(dbPath);
            lastNameRepo = new LastNameRepository(dbPath);
            nationRepo = new NationRepository(dbPath);            
            random = new Random();
        }

        public List<Player> GetAllPlayers()
        {
            return connection.GetAllWithChildren<Player>();
        }

        public Player CreatePlayer(int age)
        {
            var position = (Player.PlayerPosition)random.Next(5);
            return CreatePlayer(age, position);
        }

        public Player CreatePlayer(int age, Player.PlayerPosition position)
        {
            var nation = nationRepo.GetRandomNation();
            var firstName = firstNameRepo.GetRandomFirstNameForNation(nation);
            var lastName = lastNameRepo.GetRandomLastNameForNation(nation);

            var player = new Player() { NatonId = nation.Id, FirstNameId = firstName.Id, LastNameId = lastName.Id, Age = age };

            player.PeakAge = _meanPeakAge + (int)(_peakAgeVariance * random.NextDouble()) + position == Player.PlayerPosition.Defenseman ? _defensemanLaterPeak : 0;
            player.RetireAge = _meanRetireAge + (int)(_retireAgeVariance * random.NextDouble()) + position == Player.PlayerPosition.Defenseman ? _defensemanLaterRetirement : 0;

            player.ImproveSpeed = 0.5 + 0.5 * random.NextDouble();
            player.DeclineSpeed = (0.5 + 0.5 * random.NextDouble()) * improveContraDeclineFactor;

            player.Defense = Math.Max(0.0, position == Player.PlayerPosition.Defenseman 
                ? _strongStatMeanStart 
                : position == Player.PlayerPosition.Center
                    ? _normalStatMeanStart 
                    : _weakStatMeanStart
                +
                (position == Player.PlayerPosition.Defenseman
                ? _stableStatMean
                : _swingyStatMean) * random.NextGaussian());

            player.Endurance = Math.Max(0.0, _normalStatMeanStart + _stableStatMean * random.NextGaussian());

            player.Fitness = Math.Max(0.0, _normalStatMeanStart + _stableStatMean * random.NextGaussian());

            player.Passing = Math.Max(0.0, position == Player.PlayerPosition.Defenseman
                ? _normalStatMeanStart
                : _strongStatMeanStart
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * random.NextGaussian());

            player.PuckControl = Math.Max(0.0, position == Player.PlayerPosition.Defenseman
                ? _normalStatMeanStart
                : _strongStatMeanStart
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * random.NextGaussian());

            player.ReboundControl = Math.Max(0.0, _strongStatMeanStart + _stableStatMean * random.NextGaussian());

            player.Saving = Math.Max(0.0, _strongStatMeanStart + _stableStatMean * random.NextGaussian());

            player.Shooting = Math.Max(0.0, position == Player.PlayerPosition.Defenseman
                ? _normalStatMeanStart
                : _strongStatMeanStart
                +
                (position == Player.PlayerPosition.Defenseman
                ? _swingyStatMean
                : _stableStatMean) * random.NextGaussian());

            player.Skating = Math.Max(0.0, (position == Player.PlayerPosition.LeftWing || position == Player.PlayerPosition.RightWing)
                ? _strongStatMeanStart
                : _normalStatMeanStart
                +
                _stableStatMean * random.NextGaussian());

            return InsertPlayer(player);
        }

        private Player InsertPlayer(int firstNameId, int lastNameId)
        {
            try
            {                
                var player = new Player() { FirstNameId = firstNameId, LastNameId = lastNameId, Seasons = new List<Season>(), Matches = new List<Match>() };
                connection.Insert(player);
                return player;
            }
            catch (Exception ex)
            {
                return null;
            }
        }     
        
        private Player InsertPlayer(Player player)
        {
            try
            {
                connection.Insert(player);
                return player;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
