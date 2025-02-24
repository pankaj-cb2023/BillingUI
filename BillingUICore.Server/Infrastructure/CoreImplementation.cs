using BillingUI.Settings;

namespace BillingUICore.Server.Infrastructure
{
    public class CoreImplementation: BaseImplementation
    {
        public CoreImplementation(IRepository repository, IHttpContextAccessor contextAccessor,IHostEnvironment hostEnvironment)
         : base(repository, contextAccessor,hostEnvironment)
        {
        }

    }
}

