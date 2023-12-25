namespace UserService.API.Contract.Users;

public class UserResponse
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
	public int Balance { get; set; }

	public static UserResponse From(User user) =>
		new UserResponse
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Balance = user.Balance
		};

}
