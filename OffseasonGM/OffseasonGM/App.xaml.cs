using OffseasonGM.Assets.Managers;
using OffseasonGM.Assets.Repositories;
using OffseasonGM.Extensions;
using OffseasonGM.Global;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace OffseasonGM
{
    public partial class App : Application
    {
        public App(string dbPath)
        {
            InitializeComponent();

            var manager = new GameManager(new RepositoryLocator(), dbPath);
            manager.CreateLeague(League.LeagueConfiguration.Teams30Divisions4, 2016);
            manager.SetupLeague();
            manager.PlaySeason();

            MainPage = new TeamsPage(manager.Teams);
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
    }
}
