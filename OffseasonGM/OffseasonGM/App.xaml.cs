using OffseasonGM.Assets.Repositories;

using Xamarin.Forms;

namespace OffseasonGM
{
    public partial class App : Application
    {        
        public static GeneralRepository GeneralRepo { get; private set; }
        public static NationReposistory NationRepo { get; private set; }
        public static FirstNameRepository FirstNameRepo { get; private set; }
        public static LastNameRepository LastNameRepo { get; private set; }
        public static CityRepository CityRepo { get; private set; }
        public static NickNameRepository NickNameRepo { get; private set; }

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
            NationRepo = new NationReposistory(dbPath);
            FirstNameRepo = new FirstNameRepository(dbPath);
            LastNameRepo = new LastNameRepository(dbPath);
            CityRepo = new CityRepository(dbPath);
            NickNameRepo = new NickNameRepository(dbPath);
        }
    }
}
