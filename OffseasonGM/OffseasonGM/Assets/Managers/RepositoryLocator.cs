using OffseasonGM.Assets.Repositories;
using OffseasonGM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Managers
{
    public class RepositoryLocator : IRepositoryLocator
    {
        private readonly Dictionary<Type, Func<string, IRepository>> repositories = new Dictionary<Type, Func<string, IRepository>>()
        {
            { typeof(City), (dbPath) => new CityRepository(dbPath) },
            { typeof(Division), (dbPath) => new DivisionRepository(dbPath) },
            { typeof(FirstName), (dbPath) => new FirstNameRepository(dbPath) },
            { typeof(Goal), (dbPath) => new GoalRepository(dbPath) },
            { typeof(LastName), (dbPath) => new LastNameRepository(dbPath) },
            { typeof(League), (dbPath) => new LeagueRepository(dbPath) },
            { typeof(Match), (dbPath) => new MatchRepository(dbPath) },
            { typeof(Nation), (dbPath) => new NationRepository(dbPath) },
            { typeof(NickName), (dbPath) => new NickNameRepository(dbPath) },
            { typeof(Player), (dbPath) => new PlayerRepository(dbPath) },
            { typeof(Season), (dbPath) => new SeasonRepository(dbPath) },
            { typeof(Team), (dbPath) => new TeamRepository(dbPath) }
        };

        public void Register<T>(Func<string, IRepository> repositoryResolver)
        {
            repositories[typeof(T)] = (dbPath) => repositoryResolver(dbPath);
        }

        public T Resolve<T>(string dbPath)
        {
            return (T)repositories[typeof(T)](dbPath);
        }
    }
}
