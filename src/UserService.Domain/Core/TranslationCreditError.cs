namespace UserService.Domain.Core;

public class TranslationCreditError : Error
{
    /// <summary>
    /// Credit value object
    /// </summary>
    public string ValueObject { get; set; } = null!;

    /// <summary>
    /// Property which must be valid
    /// </summary>
    public string Property { get; set; } = null!;

    public TranslationCreditError(string message, string valueObject, string property)
        : base(message)
    {
        ValueObject = valueObject;
        Property = property;
    }
}