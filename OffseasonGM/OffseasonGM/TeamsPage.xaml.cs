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
    public partial class TeamsPage : ContentPage
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

        public TeamsPage(List<Team> teams)
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
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            var rowCount = 0;

            foreach (var goalie in CurrentTeam.GoalieOrdering.Take(2))
            {
                var positionLabel = new Label { Text = Assets.Resources.Default.G, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(goalie.Nation)) };
                var nameLabel = new Label { Text = goalie.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, rowCount);
                PlayerGrid.Children.Add(flag, 1, rowCount);
                PlayerGrid.Children.Add(nameLabel, 2, rowCount);

                rowCount++;
            }

            foreach (var defenseman in CurrentTeam.DefenseManOrdering.Take(6))
            {
                var positionLabel = new Label { Text = Assets.Resources.Default.D, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(defenseman.Nation)) };
                var nameLabel = new Label { Text = defenseman.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, rowCount);
                PlayerGrid.Children.Add(flag, 1, rowCount);
                PlayerGrid.Children.Add(nameLabel, 2, rowCount);

                rowCount++;
            }

            foreach (var center in CurrentTeam.CenterOrdering.Take(4))
            {
                var positionLabel = new Label { Text = Assets.Resources.Default.C, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(center.Nation)) };
                var nameLabel = new Label { Text = center.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, rowCount);
                PlayerGrid.Children.Add(flag, 1, rowCount);
                PlayerGrid.Children.Add(nameLabel, 2, rowCount);

                rowCount++;
            }

            foreach (var leftWing in CurrentTeam.LeftWingOrdering.Take(4))
            {
                var positionLabel = new Label { Text = Assets.Resources.Default.LW, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(leftWing.Nation)) };
                var nameLabel = new Label { Text = leftWing.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, rowCount);
                PlayerGrid.Children.Add(flag, 1, rowCount);
                PlayerGrid.Children.Add(nameLabel, 2, rowCount);

                rowCount++;
            }

            foreach (var rightWing in CurrentTeam.RightWingOrdering.Take(4))
            {
                var positionLabel = new Label { Text = Assets.Resources.Default.RW, TextColor = Color.White, BackgroundColor = Color.Black };
                var flag = new Image { Source = ImageSource.FromResource(ImageResourceNameFromNation(rightWing.Nation)) };
                var nameLabel = new Label { Text = rightWing.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };

                PlayerGrid.Children.Add(positionLabel, 0, rowCount);
                PlayerGrid.Children.Add(flag, 1, rowCount);
                PlayerGrid.Children.Add(nameLabel, 2, rowCount);

                rowCount++;
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
