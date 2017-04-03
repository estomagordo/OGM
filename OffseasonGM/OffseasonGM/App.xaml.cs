using OffseasonGM.Assets.Repositories;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace OffseasonGM
{
    public partial class App : Application
    {
        private List<Nation> _nations;
        private List<FirstName> _firstNames;
        private List<LastName> _lastNames;
        private Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)> _nationNames;

        public static GeneralRepository GeneralRepo { get; private set; }

        public static CityRepository CityRepo { get; private set; }        
        public static FirstNameRepository FirstNameRepo { get; private set; }
        public static GoalRepository GoalRepo { get; private set; }
        public static LastNameRepository LastNameRepo { get; private set; }
        public static LeagueRepository LeagueRepo { get; private set; }
        public static MatchRepository MatchRepo { get; private set; }
        public static NationRepository NationRepo { get; private set; }        
        public static NickNameRepository NickNameRepo { get; private set; }
        public static PlayerRepository PlayerRepo { get; private set; }
        public static SeasonRepository SeasonRepo { get; private set; }
        public static TeamRepository TeamRepo { get; private set; }

        private Random random;

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

        public App(string dbPath)
        {
            random = new Random();

            InitializeComponent();            
            InitializeRepositories(dbPath);
            SetNamesForNations();

            var teams = CreateTeams(30);
            var season = new Season(2016);
            
            foreach (var team in teams)
            {
                season.Teams.Add(team);
                team.Seasons.Add(season);
                AddPlayersToNewTeam(team);
                team.ArrangeBestTeam();
            }

            for (var home = 0; home < 30; home++)
            {
                for (var away = 0; away < 30; away++)
                {
                    if (home == away)
                    {
                        continue;
                    }

                    var match = new Match(season.Id, teams[home], teams[away]);
                    match.PlayGame(random);
                    season.Matches.Add(match);
                }
            }

            MainPage = new TeamsPage(teams);
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            
        }

        private void SetNamesForNations()
        {
            _nationNames = new Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)>();

            foreach(var nation in Nations)
            {
                _nationNames[nation] = (FirstNames.Where(name => name.Nations.Contains(nation)).ToList(), LastNames.Where(name => name.Nations.Contains(nation)).ToList());
            }
        }

        private Nation GetRandomNation()
        {
            return Nations[random.Next(Nations.Count)];
        }

        private FirstName GetRandomFirstNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].firstNames;
            return nameList[random.Next(nameList.Count)];
        }

        private LastName GetRandomLastNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].lastNames;
            return nameList[random.Next(nameList.Count)];
        }

        private void InitializeRepositories(string dbPath)
        {
            GeneralRepo = new GeneralRepository(dbPath);

            NationRepo = new NationRepository(dbPath);
            NickNameRepo = new NickNameRepository(dbPath);
            CityRepo = new CityRepository(dbPath);    
            FirstNameRepo = new FirstNameRepository(dbPath);
            GoalRepo = new GoalRepository(dbPath);
            LastNameRepo = new LastNameRepository(dbPath);
            LeagueRepo = new LeagueRepository(dbPath);
            MatchRepo = new MatchRepository(dbPath);            
            PlayerRepo = new PlayerRepository(dbPath);
            SeasonRepo = new SeasonRepository(dbPath);
            TeamRepo = new TeamRepository(dbPath);
        }

        private List<Team> CreateTeams(int n)
        {
            var cities = CityRepo.GetRandomSelection(n);
            var nickNames = NickNameRepo.GetRandomSelection(n);

            ShuffleList(cities);
            ShuffleList(nickNames);

            return Enumerable.Range(0, n).Select(num => new Team(cities[num], nickNames[num])).ToList();
        }

        private void AddPlayersToNewTeam(Team team)
        {
            for (var i = 0; i < 3; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var goalie = new Player(random, 18, Player.PlayerPosition.Goalie, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(goalie);
            }
            for (var i = 0; i < 8; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var defenseman = new Player(random, 18, Player.PlayerPosition.Defenseman, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(defenseman);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var center = new Player(random, 18, Player.PlayerPosition.Center, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(center);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var leftWing = new Player(random, 18, Player.PlayerPosition.LeftWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(leftWing);
            }
            for (var i = 0; i < 5; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var rightWing = new Player(random, 18, Player.PlayerPosition.RightWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(rightWing);
            }

            foreach (var player in team.Players)
            {
                var birthDays = random.Next(13);
                for (var i = 0; i < birthDays; i++)
                {
                    player.HaveBirthday(random);
                }

                player.Team = team;
                player.TeamId = team.Id;
            }
        }

        private (Nation nation, FirstName firstName, LastName lastName) GetPlayerNationAndNames()
        {
            var nation = GetRandomNation();
            var firstName = GetRandomFirstNameForNation(nation);
            var lastName = GetRandomLastNameForNation(nation);

            return (nation, firstName, lastName);
        }

        private void ShuffleList<T>(List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                Swap(list, i, random.Next(i, list.Count));
            }
        }

        private void Swap<T>(List<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
