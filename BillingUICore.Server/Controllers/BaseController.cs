using BillingUI.Settings;
using BillingUICore.Server.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillingUICore.Server.Controllers
{  
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseImplementation Implementation { get; private set; }
        public BaseController(BaseImplementation implementation)
        {
            Implementation = implementation;
        }
    }
}
