using BillingUI.Data.IRepositories;
using BillingUICore.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Repositories
{
    public class HomeRepository: IHomeRepository
    {

        // Mark Implementation as readonly to ensure it's injected via DI.
        //public BaseImplementation Implementation { get; }

        // Use DI to inject the IConfigRepository.
        private readonly IConfigRepository _config;
        public IConfigRepository Config => _config;

        // Constructor accepts dependencies via DI.
        //public HomeRepository(BaseImplementation implementation, IConfigRepository configRepository)
        //{
        //    Implementation = implementation;
        //    _config = configRepository;
        //}
    }
}

