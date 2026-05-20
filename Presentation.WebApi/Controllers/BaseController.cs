using Asp.Versioning;
using Core.Application.Helper.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
    protected ActionResult HandleResult<T>(RequestResult<T> result)
    {
        if (result is null) return NotFound();

        if (result.IsSuccess)
            return Ok(result); 

        return BadRequest(result);
    }
}
