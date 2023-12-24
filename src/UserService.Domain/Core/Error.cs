namespace UserService.Domain.Core;

public class Error
{
    /// <summary>
    /// General error message
    /// </summary>
    public string Message { get; set; }

    public Error(string message)
	{
		Message = message;
	}
}