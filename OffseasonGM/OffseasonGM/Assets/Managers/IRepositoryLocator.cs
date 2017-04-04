using OffseasonGM.Assets.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffseasonGM.Assets.Managers
{
    public interface IRepositoryLocator
    {
        void Register<T>(Func<string, IRepository> repositoryResolver);
        IRepository Resolve<T>(string dbPath);        
    }
}
