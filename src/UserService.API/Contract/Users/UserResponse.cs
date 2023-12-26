using UserService.Domain.Core;

namespace UserService.API.Contract.Users;

public class UserResponse
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string Email { get; set; } = null!;
	public int TranslationCredits { get; set; }
	public string? Tier { get; set; }

	public static UserResponse From(User user) =>
		new ()
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			TranslationCredits = user.TranslationCredits,
			Tier = user.Tier.ToString()
		};
}
