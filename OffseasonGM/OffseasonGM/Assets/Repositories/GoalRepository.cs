using OffseasonGM.Models;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OffseasonGM.Assets.Repositories
{
    public class GoalRepository
    {
        SQLiteConnection connection;

        public GoalRepository(string dbPath)
        {
            connection = new SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), dbPath);
        }

        public Goal AddNewGoal(int matchId, Player scorer, Player firstAssister, Player secondAssister)
        {
            try
            {                
                var goal = new Goal() { MatchId = matchId, Scorer = scorer };

                if (firstAssister != null)
                {
                    goal.FirstAssister = firstAssister;
                    if (secondAssister != null)
                    {
                        goal.SecondAssister = secondAssister;
                    }
                }

                connection.Insert(goal);
                return goal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Goal AddNewGoal(Goal goal)
        {
            try
            {
                connection.InsertWithChildren(goal);
                return goal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Goal> GetAllGoals()
        {
            return connection.GetAllWithChildren<Goal>();
        }
    }
}
