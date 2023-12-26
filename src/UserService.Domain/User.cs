using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public Name Name { get; protected set; }
	public Email Email { get; protected set; }
    public TranslationCredits TranslationCredits { get; protected set; }

    public User(Name name, Email email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        TranslationCredits = new TranslationCredits(0);
    }

    public static Result<User, Error> Create(string name, string email)
    {
        var nameResult = Name.Create(name);
        var emailResult = Email.Create(email);

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

    public Result<User, Error> UpdateName(Name newName)
    {
        if (newName == null)
        {
            return Result.Failure<User, Error>(new Error("Name cannot be null"));
        }
        if (newName.Value == this.Name.Value)
        {
            return Result.Success<User, Error>(this);
        }

        var nameResult = Name.Create(newName.Value);

        this.Name = nameResult.Value;
        return Result.Success<User, Error>(this);
    }

    public Result<User, Error> UpdateEmail(Email newEmail)
    {
        if (newEmail == null)
        {
            return Result.Failure<User, Error>(new Error("Email cannot be null"));
        }
        if (newEmail.Value == this.Email.Value)
        {
            return Result.Success<User, Error>(this);
        }

        var emailResult = Email.Create(newEmail.Value);
        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        this.Email = emailResult.Value;
        return Result.Success<User, Error>(this);
    }

    public Result<User, Error> AddCredits(int credits)
    {
        var result = TranslationCredits.AddCredits(credits);
        if (result.IsFailure)
        {
            return result.Error;
        }
        return Result.Success<User, Error>(this);
    }

    public Result<User, Error> SubtractCredits(int credits)
    {
        var result = TranslationCredits.SubtractCredits(credits);
        if (result.IsFailure)
        {
            return result.Error;
        }
        return Result.Success<User, Error>(this);
    }

    protected User(Result<Name, Error> result) { }
}