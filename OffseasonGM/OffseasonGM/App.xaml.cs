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

        public App(string dbPath)
        {
            random = new Random();

            InitializeComponent();
            InitializeRepositories(dbPath);

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
                var goalie = PlayerRepo.CreatePlayer(18, Player.PlayerPosition.Goalie);
                team.Players.Add(goalie);
            }
            for (var i = 0; i < 8; i++)
            {
                var defenseman = PlayerRepo.CreatePlayer(18, Player.PlayerPosition.Defenseman);
                team.Players.Add(defenseman);
            }
            for (var i = 0; i < 5; i++)
            {
                var center = PlayerRepo.CreatePlayer(18, Player.PlayerPosition.Center);
                team.Players.Add(center);
            }
            for (var i = 0; i < 5; i++)
            {
                var leftWing = PlayerRepo.CreatePlayer(18, Player.PlayerPosition.LeftWing);
                team.Players.Add(leftWing);
            }
            for (var i = 0; i < 5; i++)
            {
                var rightWing = PlayerRepo.CreatePlayer(18, Player.PlayerPosition.RightWing);
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
