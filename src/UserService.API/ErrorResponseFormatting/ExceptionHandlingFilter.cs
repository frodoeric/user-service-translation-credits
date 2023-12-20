using UserService.API.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserService.API.ErrorResponseHandling;

public class ExceptionHandlingFilter : IActionFilter, IOrderedFilter
{
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

			// TODO: Log exception

			context.ExceptionHandled = true;
		}
	}
}
