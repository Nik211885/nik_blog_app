using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/health-check")]
public class HealthCheckController : ControllerBase
{
    [HttpGet("application")]
    public Results<Ok<string>, ProblemHttpResult> Get()
    {
        return TypedResults.Ok("Health");
    }
}