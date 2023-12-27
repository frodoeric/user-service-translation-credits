namespace UserService.Domain.Core;

public class UserValidationError : Error
{
    /// <summary>
    /// Property which must be valid
    /// </summary>
    public string Property { get; set; } = null!;

    public UserValidationError(string message, string property)
        : base(message)
    {
        Property = property;
    }
}