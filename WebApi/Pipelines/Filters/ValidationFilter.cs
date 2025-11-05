using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Pipelines.Filters;

public sealed class ValidationFilter : IActionFilter
{
    /// <summary>
    ///  Defined action with validation after call controller
    /// </summary>
    /// <param name="context">action context</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .Select(ms => new
            {
                Field = ms.Key,
                Errors = ms.Value?.Errors.Select(e => e.ErrorMessage)
            });
        context.Result = new BadRequestObjectResult(new
        {
            Message = "Validation errors",
            Errors = errors
        });
    }
    /// <summary>
    ///  Defined action after validation success
    /// </summary>
    /// <param name="context">action context</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}

public sealed class ValidationFilterAttribute : TypeFilterAttribute
{
    public ValidationFilterAttribute() : base(typeof(ValidationFilter))
    {
    }
}