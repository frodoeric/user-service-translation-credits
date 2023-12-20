using UserService.Domain.ValueObjects;

namespace UserService.Domain;

public class User : Entity
{
	public static IUserRepository Repository { get; set; }
	public Name Name { get; protected set; }
	public Email Email { get; protected set; }

	public User(Name name, Email email)
	{
		Name = name;
		Email = email;
	}

	public static User Create(Name name, Email email)
	{
		var allUsers = Repository.GetAll();
		if (allUsers.Any(u => u.Email == email))
			throw new Exception("Email is duplicated");

		var user = new User(name, email);
		Repository.Add(user);
		Repository.Save();
		return user;
	}

	protected User() { }
}