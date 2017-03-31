using OffseasonGM.Assets.Repositories;
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
            LongTeamNameLabel.Text = CurrentTeam.ToString();
            TeamRecordLabel.Text = CurrentTeam.FormatedSeasonRecord;
        }

        private void CycleBackward(object sender, EventArgs e)
        {
            currentTeamIndex--;
        }

        private void CycleForward(object sender, EventArgs e)
        {
            currentTeamIndex++;
        }
    }
}
