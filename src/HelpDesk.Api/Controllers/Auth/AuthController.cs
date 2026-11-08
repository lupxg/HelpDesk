using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers.Auth;


[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync()
    {
        return Ok("Ok");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        return Ok("Ok");
    }
}
