using OffseasonGM.Assets.Repositories;
using OffseasonGM.Models;
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

        public App(string dbPath)
        {
            InitializeComponent();
            InitializeRepositories(dbPath);

            var teams = TeamRepo.CreateTeams(30);
            var season = SeasonRepo.AddNewSeason(2016);
            
            foreach (var team in teams)
            {
                season.Teams.Add(team);
                team.Seasons.Add(season);
            }

            for (var home = 0; home < 30; home++)
            {
                for (var away = 0; away < 30; away++)
                {
                    if (home == away)
                    {
                        continue;
                    }

                    var match = MatchRepo.AddNewMatch(season.Id, teams[home], teams[away]);
                    match.PlayGame();                    

                    foreach (var goal in match.Goals)
                    {
                        GoalRepo.AddNewGoal(goal);
                    }

                    MatchRepo.UpdateMatch(match);
                    season.Matches.Add(match);
                }
            }

            foreach (var team in teams)
            {
                TeamRepo.UpdateTeam(team);
            }

            SeasonRepo.UpdateSeason(season);

            MainPage = new TeamsPage(dbPath, teams.First().Id);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
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
    }
}
