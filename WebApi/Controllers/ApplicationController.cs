using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    [HttpGet("")]
    public Results<Ok<object>, ProblemHttpResult> Get()
    {
        object appInfo = new { AppName = "Nik Application" };
        return TypedResults.Ok(appInfo);
    }
}