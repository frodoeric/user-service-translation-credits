using CSharpFunctionalExtensions;
using System.Xml.Linq;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public static IUserRepository Repository { get; set; }
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

        var allUsers = Repository.GetAll();

        if (allUsers.Any(u => u.Name == this.Name && u.Id != this.Id))
        {
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "Another user with the same Name already exists.", nameof(User), nameof(User.Name)));
        }

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

        if (Repository.GetAll().Any(u => u.Email == newEmail && u.Id != this.Id))
        {
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "Email already in use by another user.", nameof(User), nameof(User.Email)));
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

    protected User() { }
}