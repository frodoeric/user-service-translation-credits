using UserService.API.Contract;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.ErrorResponseHandling;

public class InvalidModelStateResponseFactory
{
	public static IActionResult Create(ActionContext context)
	{
		var response = new ErrorResponse(
			"MODEL_BINDING_ERROR",
			string.Join(" | ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
		return new ObjectResult(response)
		{
			StatusCode = StatusCodes.Status400BadRequest
		};
	}
}
