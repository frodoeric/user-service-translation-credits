namespace UserService.Domain.Core;

public class TranslationCreditError : Error
{
    /// <summary>
    /// Property which must be valid
    /// </summary>
    public string Property { get; set; } = null!;

    public TranslationCreditError(string message, string property)
        : base(message)
    {
        Property = property;
    }
}