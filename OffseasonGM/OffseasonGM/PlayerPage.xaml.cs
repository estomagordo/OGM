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
            Title = CurrentPlayer.ToString();
            PositionLabel.Text = CurrentPlayer.ShortPosition;
            NationFlag.Source = ImageSource.FromResource(CurrentPlayer.Nation.FlagUri);

            if (CurrentPlayer.Position == Player.PlayerPosition.Goalie)
            {
                ShowGoalieGrid();
                ShowGoalieSeasonsGrid();
            }
            else
            {
                ShowSkaterGrid();
                ShowSkaterSeasonsGrid();
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

        private void ShowSkaterSeasonsGrid()
        {
            GoalieSeasonsGrid.IsVisible = false;
            SkaterSeasonsGrid.IsVisible = true;
            SkaterSeasonsGrid.Children.Clear();
            SkaterSeasonsGrid.RowDefinitions.Clear();

            SkaterSeasonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var seasonHeaderLabel = new Label { Text = Assets.Resources.Default.Season, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterSeasonsGrid.Children.Add(seasonHeaderLabel, 0, 0);

            var gamesHeaderLabel = new Label { Text = Assets.Resources.Default.Games, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterSeasonsGrid.Children.Add(gamesHeaderLabel, 1, 0);

            var goalsHeaderLabel = new Label { Text = Assets.Resources.Default.Goals, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterSeasonsGrid.Children.Add(goalsHeaderLabel, 2, 0);

            var assistsHeaderLabel = new Label { Text = Assets.Resources.Default.Assists, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterSeasonsGrid.Children.Add(assistsHeaderLabel, 3, 0);

            var pointsHeaderLabel = new Label { Text = Assets.Resources.Default.Points, TextColor = Color.White, BackgroundColor = Color.Black };
            SkaterSeasonsGrid.Children.Add(pointsHeaderLabel, 4, 0);

            foreach (var seasonStats in CurrentPlayer.SkaterSeasonStats)
            {
                SkaterSeasonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var rowCount = SkaterSeasonsGrid.RowDefinitions.Count();

                var seasonLabel = new Label { Text = seasonStats.season.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                SkaterSeasonsGrid.Children.Add(seasonLabel, 0, rowCount - 1);

                var matchesLabel = new Label { Text = seasonStats.matchesPlayed.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                SkaterSeasonsGrid.Children.Add(matchesLabel, 1, rowCount - 1);

                var goalsLabel = new Label { Text = seasonStats.goalCount.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                SkaterSeasonsGrid.Children.Add(goalsLabel, 2, rowCount - 1);

                var assistsLabel = new Label { Text = seasonStats.assistCount.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                SkaterSeasonsGrid.Children.Add(assistsLabel, 3, rowCount - 1);

                var pointsLabel = new Label { Text = seasonStats.pointCount.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                SkaterSeasonsGrid.Children.Add(pointsLabel, 4, rowCount - 1);
            }
        }

        private void ShowGoalieSeasonsGrid()
        {
            SkaterSeasonsGrid.IsVisible = false;
            GoalieSeasonsGrid.IsVisible = true;
            GoalieSeasonsGrid.Children.Clear();
            GoalieSeasonsGrid.RowDefinitions.Clear();

            GoalieSeasonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var seasonHeaderLabel = new Label { Text = Assets.Resources.Default.Season, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieSeasonsGrid.Children.Add(seasonHeaderLabel, 0, 0);

            var gamesHeaderLabel = new Label { Text = Assets.Resources.Default.Games, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieSeasonsGrid.Children.Add(gamesHeaderLabel, 1, 0);

            var savePercentageHeaderLabel = new Label { Text = Assets.Resources.Default.SavePercentage, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieSeasonsGrid.Children.Add(savePercentageHeaderLabel, 2, 0);

            var gaaHeaderLabel = new Label { Text = Assets.Resources.Default.GAA, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieSeasonsGrid.Children.Add(gaaHeaderLabel, 3, 0);

            var shutoutsHeaderLabel = new Label { Text = Assets.Resources.Default.Shutouts, TextColor = Color.White, BackgroundColor = Color.Black };
            GoalieSeasonsGrid.Children.Add(shutoutsHeaderLabel, 4, 0);

            foreach (var seasonStats in CurrentPlayer.GoalieSeasonStats)
            {
                GoalieSeasonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var rowCount = GoalieSeasonsGrid.RowDefinitions.Count();

                var seasonLabel = new Label { Text = seasonStats.season.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                GoalieSeasonsGrid.Children.Add(seasonLabel, 0, rowCount - 1);

                var matchesLabel = new Label { Text = seasonStats.matchesPlayed.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                GoalieSeasonsGrid.Children.Add(matchesLabel, 1, rowCount - 1);

                var savePercentageLabel = new Label { Text = seasonStats.savePercentage.ToTwoDecimalString(), BackgroundColor = Color.Black, TextColor = Color.White };
                GoalieSeasonsGrid.Children.Add(savePercentageLabel, 2, rowCount - 1);

                var gaaLabel = new Label { Text = seasonStats.gaa.ToTwoDecimalString(), BackgroundColor = Color.Black, TextColor = Color.White };
                GoalieSeasonsGrid.Children.Add(gaaLabel, 3, rowCount - 1);

                var shutoutsLabel = new Label { Text = seasonStats.shutouts.ToString(), BackgroundColor = Color.Black, TextColor = Color.White };
                GoalieSeasonsGrid.Children.Add(shutoutsLabel, 4, rowCount - 1);
            }
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