using OffseasonGM.Assets.Repositories;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OffseasonGM
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamPage : ContentPage
    {
        private int _currentTeamIndex;
        private List<Team> teams;

        private int currentTeamIndex
        {
            get
            {
                return _currentTeamIndex;
            }
            set
            {
                _currentTeamIndex = value % teams.Count;
                RefreshView();
            }
        }

        private Team CurrentTeam
        {
            get
            {
                return teams[currentTeamIndex];
            }
        }

        public TeamPage(List<Team> teams)
        {
            this.teams = teams;
            InitializeComponent();
            currentTeamIndex = 0;
        }

        private void RefreshView()
        {
            LongTeamNameLabel.Text = string.Format("{0} ({1})", CurrentTeam.ToString(), CurrentTeam.FormatedSeasonRecord);
            FillPlayerGrid();
        }

        private void FillPlayerGrid()
        {            
            for (var i = 0; i < CurrentTeam.Lineup.Count; i++)
            {
                var player = CurrentTeam.Lineup[i];
                var positionLabel = new Label { Text = player.ShortPosition, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(player.Nation)), BackgroundColor = Color.Black };
                var nameLabel = new Label { Text = player.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, i);
                PlayerGrid.Children.Add(flag, 1, i);
                PlayerGrid.Children.Add(nameLabel, 2, i);


            }
        }

        private void CycleBackward(object sender, EventArgs e)
        {
            currentTeamIndex--;
        }

        private void CycleForward(object sender, EventArgs e)
        {
            currentTeamIndex++;
        }

        private string ImageResourceNameFromNation(Nation nation)
        {
            return string.Format("OffseasonGM.Assets.Images.{0}_flat.png", nation.Name.ToLower());
        }
    }
}
