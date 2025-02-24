using BillingUI.Business.IService;
using BillingUI.Business.Services;
using BillingUI.Common.Model;
using BillingUI.Settings;
using BillingUICore.Server.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BillingUICore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;


        public HomeController(ILogger<HomeController> logger,IUserService userService, IConfiguration configuration)        
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("getUser")]        
        public async Task<IActionResult> GetUser()
        {
            if (HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                string appName = _configuration.GetSection("AppSettings")["TestUser"];

                var userName = HttpContext.User.Identity.Name;                
                var userExists = await _userService.GetUser(userName);
                return userExists.HasError ? BadRequest(userExists.Data) : Ok(userExists.Data);
            }
            else
            {
                return Unauthorized(new { message = "User is not authenticated", exists = false });
            }
        }

        //[HttpGet("Settings")]
        //public async Task<IActionResult> GetModule()
        //{
        //    var user = HttpContext?.User?.Identity?.Name;
        //    var result = await Implementation.GetModules();
        //    return null;
        //}




    }
}
