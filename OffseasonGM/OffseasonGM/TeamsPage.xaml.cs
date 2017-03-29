﻿using OffseasonGM.Assets.Repositories;
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
        private TeamRepository teamRepo;
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

        public TeamsPage(string dbPath, int teamId)
        {
            teamRepo = new TeamRepository(dbPath);
            teams = teamRepo.GetAllTeams();            

            InitializeComponent();

            currentTeamIndex = teams.FindIndex(team => team.Id == teamId);
        }

        private void RefreshView()
        {
            LongTeamNameLabel.Text = CurrentTeam.ToString();
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

    //class TeamsPageViewModel //: INotifyPropertyChanged
    //{

    //    public TeamsPageViewModel()
    //    {
    //        //IncreaseCountCommand = new Command(IncreaseCount);
    //    }

    //    //int count;

    //    //string countDisplay = "You clicked 0 times.";
    //    //public string CountDisplay
    //    //{
    //    //    get { return countDisplay; }
    //    //    set { countDisplay = value; OnPropertyChanged(); }
    //    //}

    //    public ICommand CycleBackCommand { get; }
    //    public ICommand CycleForwardCommand { get; }

    //    //void IncreaseCount() =>
    //    //    CountDisplay = $"You clicked {++count} times";


    //    //public event PropertyChangedEventHandler PropertyChanged;
    //    //void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
    //    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //}
}