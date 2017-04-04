using OffseasonGM.Assets.Repositories;
using OffseasonGM.Extensions;
using OffseasonGM.Global;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Managers
{
    public class GameManager
    {
        private List<Nation> _nations;
        private List<FirstName> _firstNames;
        private List<LastName> _lastNames;
        private Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)> _nationNames;
        private League _league;
        private string _dbPath;

        private static GeneralRepository GeneralRepo { get; set; }        
        private static CityRepository CityRepo { get; set; }
        private static FirstNameRepository FirstNameRepo { get; set; }
        private static GoalRepository GoalRepo { get; set; }
        private static LastNameRepository LastNameRepo { get; set; }
        private static LeagueRepository LeagueRepo { get; set; }
        private static MatchRepository MatchRepo { get; set; }
        private static NationRepository NationRepo { get; set; }
        private static NickNameRepository NickNameRepo { get; set; }
        private static PlayerRepository PlayerRepo { get; set; }
        private static SeasonRepository SeasonRepo { get; set; }
        private static TeamRepository TeamRepo { get; set; }

        public List<Nation> Nations
        {
            get
            {
                return _nations ?? (_nations = NationRepo.GetAllNations());
            }
            private set
            {
                _nations = value;
            }
        }

        public List<FirstName> FirstNames
        {
            get
            {
                return _firstNames ?? (_firstNames = FirstNameRepo.GetAllFirstNames());
            }
            private set
            {
                _firstNames = value;
            }
        }

        public List<LastName> LastNames
        {
            get
            {
                return _lastNames ?? (_lastNames = LastNameRepo.GetAllLastNames());
            }
            private set
            {
                _lastNames = value;
            }
        }

        public List<Team> Teams
        {
            get
            {
                return _league.Teams;
            }
        }

        public GameManager(string dbPath)
        {
            _dbPath = dbPath;
            InitializeRepositories();
            SetNamesForNations();
        }       
        
        public void CreateLeague(League.LeagueConfiguration configuration, int startYear)
        {
            _league = new League(configuration, startYear);
        }

        public void SetupLeague()
        {
            var teams = CreateTeams(30);            
            _league.Teams = teams;
            _league.FillDivisions();            
        }

        public void PlaySeason()
        {
            var season = new Season(2016);
            _league.Seasons.Add(season);
            _league.PlaySeason();
        }

        private List<Team> CreateTeams(int n)
        {
            var cities = CityRepo.GetRandomSelection(n);
            var nickNames = NickNameRepo.GetRandomSelection(n);

            cities.Shuffle();
            nickNames.Shuffle();

            return Enumerable.Range(0, n).Select(num => new Team(cities[num], nickNames[num])).ToList();
        }

        private void AddPlayersToNewTeam(Team team)
        {
            for (var i = 0; i < 3; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var goalie = new Player(18, Player.PlayerPosition.Goalie, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(goalie);
            }
            for (var i = 0; i < 8; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var defenseman = new Player(18, Player.PlayerPosition.Defenseman, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(defenseman);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var center = new Player(18, Player.PlayerPosition.Center, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(center);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var leftWing = new Player(18, Player.PlayerPosition.LeftWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(leftWing);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var rightWing = new Player(18, Player.PlayerPosition.RightWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(rightWing);
            }

            foreach (var player in team.Players)
            {
                var birthDays = GlobalObjects.Random.Next(13);
                for (var i = 0; i < birthDays; i++)
                {
                    player.HaveBirthday();
                }

                player.Team = team;
                player.TeamId = team.Id;
            }
        }

        private Nation GetRandomNation()
        {
            return Nations[GlobalObjects.Random.Next(Nations.Count)];
        }

        private FirstName GetRandomFirstNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].firstNames;
            return nameList[GlobalObjects.Random.Next(nameList.Count)];
        }

        private LastName GetRandomLastNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].lastNames;
            return nameList[GlobalObjects.Random.Next(nameList.Count)];
        }

        private (Nation nation, FirstName firstName, LastName lastName) GetPlayerNationAndNames()
        {
            var nation = GetRandomNation();
            var firstName = GetRandomFirstNameForNation(nation);
            var lastName = GetRandomLastNameForNation(nation);

            return (nation, firstName, lastName);
        }

        private void SetNamesForNations()
        {
            _nationNames = new Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)>();

            foreach (var nation in Nations)
            {
                _nationNames[nation] = (FirstNames.Where(name => name.Nations.Contains(nation)).ToList(), LastNames.Where(name => name.Nations.Contains(nation)).ToList());
            }
        }

        private void InitializeRepositories()
        {
            GeneralRepo = new GeneralRepository(_dbPath);

            NationRepo = new NationRepository(_dbPath);
            NickNameRepo = new NickNameRepository(_dbPath);
            CityRepo = new CityRepository(_dbPath);
            FirstNameRepo = new FirstNameRepository(_dbPath);
            GoalRepo = new GoalRepository(_dbPath);
            LastNameRepo = new LastNameRepository(_dbPath);
            LeagueRepo = new LeagueRepository(_dbPath);
            MatchRepo = new MatchRepository(_dbPath);
            PlayerRepo = new PlayerRepository(_dbPath);
            SeasonRepo = new SeasonRepository(_dbPath);
            TeamRepo = new TeamRepository(_dbPath);
        }
    }
}
