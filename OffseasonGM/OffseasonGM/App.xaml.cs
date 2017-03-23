using OffseasonGM.Assets.Repositories;
using OffseasonGM.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OffseasonGM
{
    public partial class App : Application
    {        
        public static GeneralRepository GeneralRepo { get; private set; }

        public static CityRepository CityRepo { get; private set; }
        public static GoalRepository GoalRepo { get; private set; }
        public static FirstNameRepository FirstNameRepo { get; private set; }
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

            MainPage = new MainPage();
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
