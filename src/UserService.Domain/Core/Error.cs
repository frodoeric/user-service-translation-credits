namespace UserService.Domain.Core;

public class Error
{
	/// <summary>
	/// General error message
	/// </summary>
	public string Message { get; set; } = null!;

	public Error(string message)
	{
		Message = message;
	}
}