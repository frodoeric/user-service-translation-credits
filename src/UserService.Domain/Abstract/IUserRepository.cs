namespace UserService.Domain;

/// <summary>
/// Repository pattern for users, for abstracting the data acccess layer.
/// It facilitates maintence and evolution of the softwer in case of changing data persistence.
/// </summary>
public interface IUserRepository
{
	IEnumerable<User> GetAll();

	public User? Get(long id);

	void Add(User user);

	void Update(User user);

	void Save();

	public void Remove(User user);
}