using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public static IUserRepository Repository { get; set; }
	public Name Name { get; protected set; }
	public Email Email { get; protected set; }

    public User(Name name, Email email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
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


    protected User() { }
}