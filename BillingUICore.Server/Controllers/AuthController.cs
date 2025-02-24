using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("user")]
    [Authorize]
    public IActionResult GetUser()
    {
        var user = User.Identity;
        if (user == null || !user.IsAuthenticated)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            Name = user.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }


}
