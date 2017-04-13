using OffseasonGM.Assets.Repositories;
using OffseasonGM.Extensions;
using OffseasonGM.Global;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Managers
{
    public class GameManager
    {
        private List<Nation> _nations;
        private List<FirstName> _firstNames;
        private List<LastName> _lastNames;
        private Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)> _nationNames;
        private League _league;
        private string _dbPath;
        private const int _fullTeamGoalieCount = 4;
        private const int _fullTeamDefensemanCount = 10;
        private const int _fullTeamForwardPositionCount = 7;

        private GeneralRepository GeneralRepo { get; set; }        
        private CityRepository CityRepo { get; set; }
        private FirstNameRepository FirstNameRepo { get; set; }
        private GoalRepository GoalRepo { get; set; }
        private LastNameRepository LastNameRepo { get; set; }
        private LeagueRepository LeagueRepo { get; set; }
        private MatchRepository MatchRepo { get; set; }
        private NationRepository NationRepo { get; set; }
        private NickNameRepository NickNameRepo { get; set; }
        private PlayerRepository PlayerRepo { get; set; }
        private SeasonRepository SeasonRepo { get; set; }
        private TeamRepository TeamRepo { get; set; }

        private IRepositoryLocator _repositoryLocator { get; set; }

        public List<Nation> Nations
        {
            get
            {
                return _nations ?? (_nations = NationRepo.GetAllNations());
            }
            private set
            {
                _nations = value;
            }
        }

        public List<FirstName> FirstNames
        {
            get
            {
                return _firstNames ?? (_firstNames = FirstNameRepo.GetAllFirstNames());
            }
            private set
            {
                _firstNames = value;
            }
        }

        public List<LastName> LastNames
        {
            get
            {
                return _lastNames ?? (_lastNames = LastNameRepo.GetAllLastNames());
            }
            private set
            {
                _lastNames = value;
            }
        }

        public List<Team> Teams
        {
            get
            {
                return _league.Teams;
            }
        }

        public GameManager(IRepositoryLocator repositoryLocator, string dbPath)
        {
            _dbPath = dbPath;
            _repositoryLocator = repositoryLocator;
            InitializeRepositories();
            SetNamesForNations();
        }       
        
        public void CreateLeague(League.LeagueConfiguration configuration, int startYear)
        {
            _league = new League(configuration, startYear);
        }

        public void SetupLeague()
        {
            var teams = CreateTeams(30);            
            _league.Teams = teams;
            _league.FillDivisions();
            SetupTeams();
        }

        public void PlaySeason()
        {
            var startYear = _league.StartYear + _league.Seasons.Count();
            var season = new Season(startYear);
            SeasonRepo.Save(season);
            _league.Seasons.Add(season);

            PlayInterConferentialGames();

            PlayDivisionalNeighbourGames(_league.WesternConference);
            PlayDivisionalNeighbourGames(_league.EasternConference);

            foreach (var division in _league.WesternConference)
            {
                PlayIntraDivisionalGames(division);
            }
            foreach (var division in _league.EasternConference)
            {
                PlayIntraDivisionalGames(division);
            }

            PlayTrailingGames();

            RoundUpSeason();

            foreach (var team in Teams)
            {
                FillUpPlayersForTeam(team, false);
                team.ArrangeBestTeam();
            }            
        }

        private void RoundUpSeason()
        {
            var lastSeason = _league.Seasons.Last();

            foreach (var team in Teams)
            {
                team.Seasons.Add(lastSeason);
                var playersToRetire = new List<Player>();
                foreach (var player in team.Players)
                {
                    player.Seasons.Add(lastSeason);
                    player.HaveBirthday();
                    if (player.Retired)
                    {
                        playersToRetire.Add(player);
                    }
                }
                foreach (var player in playersToRetire)
                {
                    team.Players.Remove(player);
                }
            }
        }

        private void PlayInterConferentialGames()
        {
            foreach (var westernDivision in _league.WesternConference)
            {
                foreach (var westernTeam in westernDivision.Teams)
                {
                    foreach (var easternDivision in _league.EasternConference)
                    {
                        foreach (var easternTeam in easternDivision.Teams)
                        {
                            PlayGame(westernTeam, easternTeam);
                            PlayGame(easternTeam, westernTeam);
                        }
                    }
                }
            }
        }

        private void PlayDivisionalNeighbourGames(List<Division> conference)
        {
            var firstHasMoreHomeGames = GlobalObjects.Random.Next(2) == 0;
            for (var i = 0; i < conference.Count - 1; i++)
            {
                for (var j = i + 1; j < conference.Count; j++)
                {
                    foreach (var iTeam in conference[i].Teams)
                    {
                        foreach (var jTeam in conference[j].Teams)
                        {
                            PlayGame(iTeam, jTeam);
                            PlayGame(jTeam, iTeam);
                            if (firstHasMoreHomeGames)
                            {
                                PlayGame(iTeam, jTeam);
                            }
                            else
                            {
                                PlayGame(jTeam, iTeam);
                            }
                            firstHasMoreHomeGames = !firstHasMoreHomeGames;
                        }
                    }
                }
            }
        }

        private void PlayIntraDivisionalGames(Division division)
        {
            var alternateTeamList = new List<Team>();
            foreach (var team in division.Teams)
            {
                alternateTeamList.Add(team);
            }
            alternateTeamList.Shuffle();

            while (alternateTeamList.Count(team => team.AwayGames.Count % 41 > 0 || team.HomeGames.Count % 41 > 0) > 1)
            {
                for (var i = 0; i < alternateTeamList.Count - 1; i++)
                {

                    var iTeam = alternateTeamList[i];
                    for (var j = i + 1; j < alternateTeamList.Count; j++)
                    {
                        var jTeam = alternateTeamList[j];
                        var iHasFewerHomeGames = iTeam.HomeGames.Count < jTeam.HomeGames.Count;
                        var equalAmountOfHomeGames = iTeam.HomeGames.Count == jTeam.HomeGames.Count;
                        var tails = GlobalObjects.Random.Next(2) == 0;
                        var iPreferedHomeTeam = iHasFewerHomeGames || (equalAmountOfHomeGames && tails);

                        var iHasHomeGamesRemaining = iTeam.HomeGames.Count % 41 > 0;
                        var iHasAwayGamesRemaining = iTeam.AwayGames.Count % 41 > 0;
                        var jHasHomeGamesRemaining = jTeam.HomeGames.Count % 41 > 0;
                        var jHasAwayGamesRemaining = jTeam.AwayGames.Count % 41 > 0;
                        var canPlayIJ = iHasHomeGamesRemaining && jHasAwayGamesRemaining;
                        var canPlayJI = jHasHomeGamesRemaining && iHasAwayGamesRemaining;

                        if (canPlayIJ && (iPreferedHomeTeam || !canPlayJI))
                        {
                            PlayGame(iTeam, jTeam);
                        }
                        else if (canPlayJI)
                        {
                            PlayGame(jTeam, iTeam);
                        }
                    }
                }
            }
        }

        private void PlayTrailingGames()
        {
            while (true)
            {
                var needsHomeGame = Teams.FirstOrDefault(team => team.HomeGames.Count % 41 > 0);
                if (needsHomeGame == null)
                {
                    return;
                }
                var needsAwayGame = Teams.First(team => team.AwayGames.Count % 41 > 0);
                PlayGame(needsHomeGame, needsAwayGame);
            }
        }

        private void PlayGame(Team home, Team away)
        {
            var match = new Match(_league.CurrentSeason.Id, home, away);
            MatchRepo.Save(match);
            match.PlayGame();            
            _league.CurrentSeason.Matches.Add(match);
        }

        private void SetupTeams()
        {
            foreach (var team in Teams)
            {
                FillUpPlayersForTeam(team, true);
                team.ArrangeBestTeam();
            }
        }

        private List<Team> CreateTeams(int n)
        {
            var cities = CityRepo.GetRandomSelection(n);
            var nickNames = NickNameRepo.GetRandomSelection(n);

            cities.Shuffle();
            nickNames.Shuffle();

            return Enumerable.Range(0, n).Select(num =>
            {
                var team = new Team(cities[num], nickNames[num]);
                TeamRepo.Save(team);
                return team;
            }).ToList();
        }

        private void FillUpPlayersForTeam(Team team, bool autoAge)
        {
            var goalieCount = team.Players.Count(player => player.Position == Player.PlayerPosition.Goalie);
            var defenseManCount = team.Players.Count(player => player.Position == Player.PlayerPosition.Defenseman);
            var centerCount = team.Players.Count(player => player.Position == Player.PlayerPosition.Center);
            var leftWingCount = team.Players.Count(player => player.Position == Player.PlayerPosition.LeftWing);
            var rightWingCount = team.Players.Count(player => player.Position == Player.PlayerPosition.RightWing);

            for (var i = goalieCount; i < _fullTeamGoalieCount; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var goalie = new Player(18, Player.PlayerPosition.Goalie, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(goalie);
            }
            for (var i = defenseManCount; i < _fullTeamDefensemanCount; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var defenseman = new Player(18, Player.PlayerPosition.Defenseman, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(defenseman);
            }
            for (var i = centerCount; i < _fullTeamForwardPositionCount; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var center = new Player(18, Player.PlayerPosition.Center, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(center);
            }
            for (var i = leftWingCount; i < _fullTeamForwardPositionCount; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var leftWing = new Player(18, Player.PlayerPosition.LeftWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(leftWing);
            }
            for (var i = rightWingCount; i < _fullTeamForwardPositionCount; i++)
            {
                var nationAndNames = GetPlayerNationAndNames();
                var rightWing = new Player(18, Player.PlayerPosition.RightWing, nationAndNames.nation, nationAndNames.firstName, nationAndNames.lastName);
                team.Players.Add(rightWing);
            }

            if (!autoAge)
            {
                return;
            }

            foreach (var player in team.Players)
            {
                var birthDays = GlobalObjects.Random.Next(13);
                for (var i = 0; i < birthDays; i++)
                {
                    player.HaveBirthday();
                }

                player.Team = team;
                player.TeamId = team.Id;

                PlayerRepo.Save(player);
            }
        }

        private Nation GetRandomNation()
        {
            var nationRoll = GlobalObjects.Random.NextDouble();
            var accumulated = 0.0;

            foreach (var nation in Nations)
            {
                accumulated += nation.Frequency;
                if (nationRoll < accumulated)
                {
                    return nation;
                }
            }

            return Nations.Last();
        }

        private FirstName GetRandomFirstNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].firstNames;
            return nameList[GlobalObjects.Random.Next(nameList.Count)];
        }

        private LastName GetRandomLastNameForNation(Nation nation)
        {
            var nameList = _nationNames[nation].lastNames;
            return nameList[GlobalObjects.Random.Next(nameList.Count)];
        }

        private (Nation nation, FirstName firstName, LastName lastName) GetPlayerNationAndNames()
        {
            var nation = GetRandomNation();
            var firstName = GetRandomFirstNameForNation(nation);
            var lastName = GetRandomLastNameForNation(nation);

            return (nation, firstName, lastName);
        }

        private void SetNamesForNations()
        {
            _nationNames = new Dictionary<Nation, (List<FirstName> firstNames, List<LastName> lastNames)>();

            foreach (var nation in Nations)
            {
                _nationNames[nation] = (FirstNames.Where(name => name.Nations.Contains(nation)).ToList(), LastNames.Where(name => name.Nations.Contains(nation)).ToList());
            }
        }

        private void InitializeRepositories()
        {
            GeneralRepo = new GeneralRepository(_dbPath);

            CityRepo = _repositoryLocator.Resolve<City>(_dbPath) as CityRepository;
            NationRepo = _repositoryLocator.Resolve<Nation>(_dbPath) as NationRepository;
            FirstNameRepo = _repositoryLocator.Resolve<FirstName>(_dbPath) as FirstNameRepository;
            LastNameRepo = _repositoryLocator.Resolve<LastName>(_dbPath) as LastNameRepository;            
            NickNameRepo = _repositoryLocator.Resolve<NickName>(_dbPath) as NickNameRepository;
            TeamRepo = _repositoryLocator.Resolve<Team>(_dbPath) as TeamRepository;
            SeasonRepo = _repositoryLocator.Resolve<Season>(_dbPath) as SeasonRepository;
            MatchRepo = _repositoryLocator.Resolve<Match>(_dbPath) as MatchRepository;
            PlayerRepo = _repositoryLocator.Resolve<Player>(_dbPath) as PlayerRepository;
        }
    }
}
