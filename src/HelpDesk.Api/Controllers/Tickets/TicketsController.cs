using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers.Tickets;

[ApiController]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TicketsController : Controller
{
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTikesAsync()
    {
        return Ok("tickets");
    }
}
