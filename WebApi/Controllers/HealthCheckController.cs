using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/health-check")]
public class HealthCheckController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthCheckController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("application")]
    public Results<Ok<string>, ProblemHttpResult> GetHealthApplication()
    {
        return TypedResults.Ok("Health!");
    }

    [HttpGet("postgres")]
    public async Task<Results<Ok<string>, InternalServerError<string>, ProblemHttpResult>> GetHealthPostgres()
    {
        if (await _context.Database.CanConnectAsync())
        {
            return TypedResults.Ok("Postgres, Health!");
        }
        return TypedResults.InternalServerError("Postgres, Disconnection!");
    }
}