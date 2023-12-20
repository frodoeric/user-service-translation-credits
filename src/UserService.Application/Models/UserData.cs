namespace UserService.Application.Models;

public abstract class UserData
{
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
}
