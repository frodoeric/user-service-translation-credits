using UserService.Domain.Core;

namespace UserService.API.Contract;

public class ErrorResponse
{
	/// <summary>
	/// Error code
	/// </summary>
	public string Code { get; set; } = null!;

	/// <summary>
	/// General error message
	/// </summary>
	public string Message { get; set; } = null!;

	/// <summary>
	/// Details about the error
	/// </summary>
	public object? Data { get; set; }

	public ErrorResponse(string code, string message, object? data = null)
	{
		Code = code;
		Message = message;
		Data = data;
	}

	public static ErrorResponse EntityNotFound(string? type = "entity") =>
		new("ENTITY_NOT_FOUND", $"The {type} was not found");

	public static ErrorResponse ValidationError(string message) =>
		new("VALIDATION_ERROR", message);

	public static ErrorResponse From(Error error)
	{
		if (error is UniqueConstraintViolationError constraintViolationError)
			return new ErrorResponse("UNIQUE_CONSTRAINT", constraintViolationError.Message, constraintViolationError);
		if (error is UserValidationError validationError)
            return new ErrorResponse("VALIDATION_ERROR", validationError.Message, validationError);
        return new ErrorResponse("UNKNOWN_ERROR", error.Message);
	}
}