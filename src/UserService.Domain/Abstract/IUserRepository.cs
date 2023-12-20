namespace UserService.Domain;

public interface IUserRepository
{
	IEnumerable<User> GetAll();

	void Add(User user);

	void Save();
}