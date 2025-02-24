using BillingUI.Business.IService;
using BillingUI.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace BillingUICore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IEventService eventService, IUserService userService)
        {
            _userService = userService;
            _eventService = eventService;
            _logger = logger;
        }
         
        [HttpPost("searchUser")]
        public async Task<IActionResult> SearchUsers(UserModel searchTerm)
        {
            var result = await _userService.SearchUsersAsync(searchTerm);
            return result.HasError ? BadRequest(result.Data) : Ok(result.Data);
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRole([FromQuery] int? roleId = null)
        {
            var result = await _userService.GetRoles(roleId);
            return result.HasError ? BadRequest(result.Data) : Ok(result.Data);
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(AddUserRequestModel addUser)
        {
            var result = await _userService.AddUsersAsync(addUser);
            return result.HasError ? BadRequest(result.Data) : Ok(result.Data);
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int userId)
        {
            var result = await _userService.DeleteUser(userId);
            return result.HasError ? BadRequest(result.Data) : Ok(result.Data);
        }

    }
}
