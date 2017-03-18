using OffseasonGM.Assets.Repositories;

using Xamarin.Forms;

namespace OffseasonGM
{
    public partial class App : Application
    {
        public static NationReposistory NationRepo { get; private set; }
        public static FirstNameRepository FirstNameRepo { get; private set; }

        public App(string dbPath)
        {
            InitializeComponent();

            NationRepo = new NationReposistory(dbPath);
            FirstNameRepo = new FirstNameRepository(dbPath);

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
    }
}
