using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class MetaController : ControllerBase
{
    /// <summary>
    /// Get metadata of the assembly.
    /// </summary>
    /// <returns>a string containg the version and the last update of the Assembly</returns>
    [HttpGet("info")]
    public ActionResult<string> Info()
    {
        var assembly = typeof(Program).Assembly;

        var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        var version = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        
        return Ok($"Version: {version}, Last update {lastUpdate}");
    }
}
