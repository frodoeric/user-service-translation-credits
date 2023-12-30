using UserService.API.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserService.API.ErrorResponseHandling;

public class ExceptionHandlingFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<ExceptionHandlingFilter> _logger;

    public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
    {
        _logger = logger;
    }

    public int Order { get; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null)
        {
            var response = new ErrorResponse("UNHANDLED_EXCEPTION", context.Exception.Message);
            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };

            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            context.ExceptionHandled = true;
        }
    }
}
