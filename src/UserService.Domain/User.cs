using System.Xml.Linq;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public static IUserRepository Repository { get; set; }
	public Name Name { get; protected set; }
	public Email Email { get; protected set; }
    public TranslationCredits Balance { get; protected set; }

    public User(Name name, Email email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Balance = new TranslationCredits(0);
    }

    public static Result<User, Error> Create(string name, string email)
    {
        var nameResult = Name.Set(name);
        var emailResult = Email.Set(email);

        if (nameResult.IsFailure || emailResult.IsFailure)
        {
            var errors = new List<string>();
            if (nameResult.IsFailure)
            {
                errors.Add(nameResult.Error.Message);
            }
            if (emailResult.IsFailure)
            {
                errors.Add(emailResult.Error.Message);
            }
            var combinedErrorMessage = string.Join(" ", errors);
            return Result.Failure<User, Error>(new Error(combinedErrorMessage));
        }

        var user = new User(nameResult.Value, emailResult.Value);

        return Result.Success<User, Error>(user);
    }

    protected User() { }
}