using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Models
{
    public class Match
    {
        public enum MatchEvent
        {
            HomeDefenseShift,
            HomeAttackShift,
            AwayDefenseShift,
            AwayAttackShift,
            Opportunity
        }
        
        private const int opportunityGap = 35;
        private const int timeVariance = 15;

        [Ignore]
        private Player homeGoalie { get; set; }

        [Ignore]
        private Player awayGoalie { get; set; }

        [Ignore]
        private Random random { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }        

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        [OneToMany]
        public List<Goal> Goals { get; set; }

        [ForeignKey(typeof(Team))]
        public int HomeTeamKey { get; set; }

        [ForeignKey(typeof(Team))]
        public int AwayTeamKey { get; set; }

        [ManyToOne("HomeTeamKey", "HomeGames")]
        public Team HomeTeam { get; set; }

        [ManyToOne("AwayTeamKey", "AwayGames")]
        public Team AwayTeam { get; set; }

        [ManyToMany(typeof(MatchPlayer))]
        public List<Player> Players { get; set; }

        public int HomeShots { get; set; }
        public int AwayShots { get; set; }
        [Ignore]
        public int HomeGoalCount
        {
            get
            {
                return Goals.Count(goal => goal.Team == HomeTeam);
            }
        }
        [Ignore]
        public int AwayGoalCount
        {
            get
            {
                return Goals.Count(goal => goal.Team == AwayTeam);
            }
        }

        public Match()
        {
            Goals = new List<Goal>();
        }

        public Match(int seasonId, Team home, Team away)
        {
            Goals = new List<Goal>();
            Players = home.Players.Concat(away.Players).ToList();
            SeasonId = seasonId;
            HomeTeam = home;
            AwayTeam = away;
        }

        public void PlayGame(Random random)
        {
            this.random = random;

            homeGoalie = HomeTeam.GoalieOrdering[random.NextDouble() <= HomeTeam.FirstGoalieShare ? 0 : 1];
            awayGoalie = AwayTeam.GoalieOrdering[random.NextDouble() <= AwayTeam.FirstGoalieShare ? 0 : 1];

            for (var i = 0; i < 3; i++)
            {
                PlayPeriod(i+1);
            }

            HomeTeam.HomeGames.Add(this);
            AwayTeam.AwayGames.Add(this);
        }

        private void PlayPeriod(int periodNumber)
        {
            var homePair = 0;
            var homeLine = 0;
            var awayPair = 0;
            var awayLine = 0;

            var events = new List<(int second, MatchEvent matchEvent)>();

            events.AddRange(GetEventsByTypeAndIntervals(MatchEvent.HomeDefenseShift, HomeTeam.DefenseShiftTimes));
            events.AddRange(GetEventsByTypeAndIntervals(MatchEvent.HomeAttackShift, HomeTeam.OffenseShiftTimes));
            events.AddRange(GetEventsByTypeAndIntervals(MatchEvent.AwayDefenseShift, AwayTeam.DefenseShiftTimes));
            events.AddRange(GetEventsByTypeAndIntervals(MatchEvent.AwayAttackShift, AwayTeam.OffenseShiftTimes));
            events.AddRange(GetEventsByTypeAndIntervals(MatchEvent.Opportunity, new List<int> { opportunityGap }));

            events.Sort();

            foreach (var me in events)
            {
                switch (me.matchEvent)
                {
                    case MatchEvent.HomeDefenseShift:
                        homePair = (homePair + 1) % 3;
                        break;
                    case MatchEvent.HomeAttackShift:
                        homeLine = (homeLine + 1) % 4;
                        break;
                    case MatchEvent.AwayDefenseShift:
                        awayPair = (homePair + 1) % 3;
                        break;
                    case MatchEvent.AwayAttackShift:
                        awayLine = (homeLine + 1) % 4;
                        break;
                    default:
                        var homeAttacks = HomeTeamAttacks(homePair, homeLine, awayPair, awayLine);
                        var shooter = GetShooter(homeAttacks ? HomeTeam : AwayTeam,
                                                 homeAttacks ? homePair : awayPair,
                                                 homeAttacks ? homeLine : awayLine);

                        var hitsTarget = ShotHitsTarget(shooter);
                        if (!hitsTarget)
                        {
                            break;
                        }

                        if (homeAttacks)
                        {
                            HomeShots += 1;
                        }
                        else
                        {
                            AwayShots += 1;
                        }

                        var shooterScores = ShotScores(shooter, homeAttacks ? awayGoalie : homeGoalie);

                        if (!shooterScores)
                        {
                            break;
                        }

                        var assistCountRoll = random.NextDouble() * 30.0;
                        var assistCount = assistCountRoll < 2.0
                            ? 0
                            : assistCountRoll < 7.0
                                ? 1
                                : 2;

                        var assisters = GetAssisters(assistCount,
                                                     homeAttacks ? HomeTeam : AwayTeam,
                                                     shooter,
                                                     homeAttacks ? homePair : awayPair,
                                                     homeAttacks ? homeLine : awayLine);

                        var goal = new Goal(homeAttacks ? HomeTeam :AwayTeam,
                                            periodNumber,
                                            me.second,
                                            shooter,
                                            assisters);

                        Goals.Add(goal);

                        break;
                }
            }
        }

        private bool HomeTeamAttacks(int homePair, int homeLine, int awayPair, int awayLine)
        {
            var homeInitiave = GetTeamInitiative(HomeTeam, homePair, homeLine);
            var homeLuck = 0.75 + random.NextDouble() / 2.0;
            var awayInitiative = GetTeamInitiative(AwayTeam, awayPair, awayLine);
            var awayLuck = 0.75 + random.NextDouble() / 2.0;

            return homeInitiave * homeLuck > awayInitiative * awayLuck;
        }

        private Player GetShooter(Team team, int pair, int line)
        {            
            var allShotInitiatives = new List<double>();

            allShotInitiatives.Add(team.DefenseManOrdering[pair * 2].Skating * 0.25 + team.DefenseManOrdering[pair * 2].Shooting * 0.125);
            allShotInitiatives.Add(team.DefenseManOrdering[pair * 2 + 1].Skating * 0.25 + team.DefenseManOrdering[pair * 2 + 1].Shooting * 0.125);
            allShotInitiatives.Add(team.CenterOrdering[line].Skating * 0.875 + team.CenterOrdering[line].Shooting * 0.5);
            allShotInitiatives.Add(team.LeftWingOrdering[line].Skating + team.LeftWingOrdering[line].Shooting * 0.5);
            allShotInitiatives.Add(team.RightWingOrdering[line].Skating + team.RightWingOrdering[line].Shooting * 0.5);

            var totalShotInitiative = allShotInitiatives.Sum();
            var initiativeRoll = totalShotInitiative * random.NextDouble();

            var cumulativeInitiative = allShotInitiatives[0];

            if (initiativeRoll <= cumulativeInitiative)
            {
                return team.DefenseManOrdering[pair * 2];
            }

            cumulativeInitiative += allShotInitiatives[1];

            if (initiativeRoll <= cumulativeInitiative)
            {
                return team.DefenseManOrdering[pair * 2 + 1];
            }

            cumulativeInitiative += allShotInitiatives[2];

            if (initiativeRoll <= cumulativeInitiative)
            {
                return team.CenterOrdering[line];
            }

            cumulativeInitiative += allShotInitiatives[3];

            return initiativeRoll <= cumulativeInitiative
                ? team.LeftWingOrdering[line]
                : team.RightWingOrdering[line];
        }

        private bool ShotHitsTarget(Player shooter)
        {
            // TODO: An actual implementaton
            return random.NextDouble() < 0.5;
        }

        private bool ShotScores(Player shooter, Player keeper)
        {
            return shooter.Shooting * random.NextDouble() > keeper.Saving * random.NextDouble() * 6.0;
        }

        private List<Player> GetAssisters(int assisterCount, Team team, Player scorer, int pair, int line)
        {
            var assisters = new List<Player>();            

            var players = new List<Player> { team.DefenseManOrdering[pair * 2],
                                             team.DefenseManOrdering[pair * 2 + 1],
                                             team.CenterOrdering[line],
                                             team.LeftWingOrdering[line],
                                             team.RightWingOrdering[line]};
            players.Remove(scorer);

            for (var i = 0; i < assisterCount; i++)
            {
                var totalPassing = players.Sum(player => (player.Position == Player.PlayerPosition.Defenseman ? 0.5 : 1.0) * player.Passing);
                var passingRoll = random.NextDouble() * totalPassing;
                var cumulativePassing = 0.0;

                Player assister = null;

                foreach (var player in players)
                {
                    cumulativePassing += (player.Position == Player.PlayerPosition.Defenseman ? 0.5 : 1.0) * player.Passing;
                    if (passingRoll < cumulativePassing)
                    {
                        assister = player;
                        break;
                    }
                }

                // This should never happen
                if (assister == null)
                {
                    assister = players.First();
                }

                assisters.Add(assister);
                players.Remove(assister);
            }

            return assisters;
        }

        private double GetTeamInitiative(Team team, int pair, int line)
        {
            return team.DefenseManOrdering[pair * 2].Defense + team.DefenseManOrdering[pair * 2].Skating + team.DefenseManOrdering[pair * 2].Passing * 0.5
                +
                team.DefenseManOrdering[pair * 2 + 1].Defense + team.DefenseManOrdering[pair * 2 + 1].Skating + team.DefenseManOrdering[pair * 2 + 1].Passing * 0.5
                +
                team.CenterOrdering[line].Passing + team.CenterOrdering[line].Skating * 0.75 + team.CenterOrdering[line].Defense * 0.5
                +
                team.LeftWingOrdering[line].Passing + team.LeftWingOrdering[line].Skating + team.LeftWingOrdering[line].Defense * 0.25
                +
                team.RightWingOrdering[line].Passing + team.RightWingOrdering[line].Skating + team.RightWingOrdering[line].Defense * 0.25;
        }

        private List<(int, MatchEvent)> GetEventsByTypeAndIntervals(MatchEvent matchEvent, List<int> intervals)
        {
            var eventList = new List<(int, MatchEvent)>();
            var second = 0;
            var position = 0;
            while (true)
            {
                second += (int)(intervals[position++ % intervals.Count] + random.NextDouble() * timeVariance - timeVariance / 2.0);
                if (second >= 1200)
                {
                    return eventList;
                }

                eventList.Add((second, matchEvent));                
            }
        }

        public override string ToString()
        {
            return HomeTeam + " (" + Goals.Where(goal => goal.Team == HomeTeam).ToString() + ") " + AwayTeam + " (" + Goals.Where(goal => goal.Team == AwayTeam).ToString() + ")";
        }
    }
}
