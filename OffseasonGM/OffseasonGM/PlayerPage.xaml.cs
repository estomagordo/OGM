using OffseasonGM.Extensions;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OffseasonGM
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        private int _currentPlayerIndex;
        private Team _currentTeam;

        private int CurrentPlayerIndex
        {
            get
            {
                return _currentPlayerIndex;
            }
            set
            {
                _currentPlayerIndex = value % _currentTeam.Lineup.Count;
                RefreshView();
            }
        }

        private Player CurrentPlayer
        {
            get
            {
                return _currentTeam.Lineup[CurrentPlayerIndex];
            }
        }

        public PlayerPage(Team team, int playerIndex)
        {
            InitializeComponent();

            _currentTeam = team;
            CurrentPlayerIndex = playerIndex;
        }

        private void RefreshView()
        {
            PositionLabel.Text = CurrentPlayer.ShortPosition;
            NationFlag.Source = ImageSource.FromResource(CurrentPlayer.Nation.FlagUri);
            NameLabel.Text = CurrentPlayer.ToString();

            if (CurrentPlayer.Position == Player.PlayerPosition.Goalie)
            {
                ShowGoalieGrid();
            }
            else
            {
                ShowSkaterGrid();
            }
        }

        private void ShowSkaterGrid()
        {
            GoalieAttributeGrid.IsVisible = false;
            SkaterAttributeGrid.IsVisible = true;
            SkaterAttributeGrid.Children.Clear();

            var ageHeader = new Label { Text = Assets.Resources.Default.Age, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(ageHeader, 0, 0);

            var ageValue = new Label { Text = CurrentPlayer.Age.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(ageValue, 1, 0);

            var defenseHeader = new Label { Text = Assets.Resources.Default.Defense, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(defenseHeader, 0, 1);

            var defenseValue = new Label { Text = CurrentPlayer.Defense.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(defenseValue, 1, 1);

            var enduranceHeader = new Label { Text = Assets.Resources.Default.Endurance, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(enduranceHeader, 2, 1);

            var enduranceValue = new Label { Text = CurrentPlayer.Endurance.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(enduranceValue, 3, 1);

            var fitnessHeader = new Label { Text = Assets.Resources.Default.Fitness, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(fitnessHeader, 0, 2);

            var fitnessValue = new Label { Text = CurrentPlayer.Fitness.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(fitnessValue, 1, 2);

            var passingHeader = new Label { Text = Assets.Resources.Default.Passing, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(passingHeader, 2, 2);

            var passingValue = new Label { Text = CurrentPlayer.Passing.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(passingValue, 3, 2);

            var puckControlHeader = new Label { Text = Assets.Resources.Default.PuckControl, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(puckControlHeader, 0, 3);

            var puckControlValue = new Label { Text = CurrentPlayer.PuckControl.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(puckControlValue, 1, 3);

            var shootingHeader = new Label { Text = Assets.Resources.Default.Shooting, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(shootingHeader, 2, 3);

            var shootingValue = new Label { Text = CurrentPlayer.Shooting.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(shootingValue, 3, 3);

            var skatingHeader = new Label { Text = Assets.Resources.Default.Skating, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(skatingHeader, 0, 4);

            var skatingValue = new Label { Text = CurrentPlayer.Skating.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterAttributeGrid.Children.Add(skatingValue, 1, 4);
        }

        private void ShowGoalieGrid()
        {
            SkaterAttributeGrid.IsVisible = false;
            GoalieAttributeGrid.IsVisible = true;
            GoalieAttributeGrid.Children.Clear();

            var ageHeader = new Label { Text = Assets.Resources.Default.Age, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(ageHeader, 0, 0);

            var ageValue = new Label { Text = CurrentPlayer.Age.ToString(), TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(ageValue, 1, 0);

            var enduranceHeader = new Label { Text = Assets.Resources.Default.Endurance, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(enduranceHeader, 0, 1);

            var enduranceValue = new Label { Text = CurrentPlayer.Endurance.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(enduranceValue, 1, 1);

            var fitnessHeader = new Label { Text = Assets.Resources.Default.Fitness, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(fitnessHeader, 2, 1);

            var fitnessValue = new Label { Text = CurrentPlayer.Fitness.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(fitnessValue, 3, 1);

            var reboundControlHeader = new Label { Text = Assets.Resources.Default.ReboundControl, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(reboundControlHeader, 0, 2);

            var reboundControlValue = new Label { Text = CurrentPlayer.ReboundControl.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(reboundControlValue, 1, 2);

            var savingHeader = new Label { Text = Assets.Resources.Default.Saving, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(savingHeader, 2, 2);

            var savingValue = new Label { Text = CurrentPlayer.Saving.ToRoundedString(), TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieAttributeGrid.Children.Add(savingValue, 3, 2);
        }

        private async void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void CycleBackward(object sender, EventArgs e)
        {
            CurrentPlayerIndex--;
        }

        private void CycleForward(object sender, EventArgs e)
        {
            CurrentPlayerIndex++;
        }
    }
}