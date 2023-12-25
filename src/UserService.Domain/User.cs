using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public static IUserRepository Repository { get; set; }
	public Name Name { get; protected set; }
	public Email Email { get; protected set; }
    public TranslationCredits Balance { get; private set; }

    public User(Name name, Email email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Balance = new TranslationCredits(0);
    }

    public static Result<User, Error> Create(Name name, Email email)
    {
        var allUsers = Repository.GetAll();
        if (allUsers.Any(u => u.Email == email))
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "User with given Email already exists.", nameof(User), nameof(User.Email)));

        var user = new User(name, email);
        Repository.Add(user);
        Repository.Save();
        return Result.Success<User, Error>(user);
    }

    public Result<User, Error> UpdateName(Name newName)
    {
        if (newName == null)
        {
            return Result.Failure<User, Error>(new Error("Name cannot be null"));
        }

        var allUsers = Repository.GetAll();

        if (allUsers.Any(u => u.Name == this.Name && u.Id != this.Id))
        {
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "Another user with the same Name already exists.", nameof(User), nameof(User.Name)));
        }

        this.Name = newName;
        return Result.Success<User, Error>(this);
    }

    public Result<User, Error> UpdateEmail(Email newEmail)
    {
        if (newEmail == null)
        {
            return Result.Failure<User, Error>(new Error("Email cannot be null"));
        }

        if (Repository.GetAll().Any(u => u.Email == newEmail && u.Id != this.Id))
        {
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "Email already in use by another user.", nameof(User), nameof(User.Email)));
        }

        this.Email = newEmail;
        return Result.Success<User, Error>(this);
    }

   

    protected User() { }
}