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
            
            RefreshView();
        }

        private void RefreshView()
        {
            PositionLabel.Text = CurrentPlayer.ShortPosition;
            NationFlag.Source = ImageSource.FromResource(CurrentPlayer.Nation.FlagUri);
            NameLabel.Text = CurrentPlayer.ToString();
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
