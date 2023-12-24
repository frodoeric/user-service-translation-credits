using UserService.Application.Models;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Services;

namespace UserService.Application;

public class UserCreator
{
	private readonly IUserRepository userRepository;
    private readonly ICrmService crmService;

	public UserCreator(IUserRepository userRepository, ICrmService crmService)
    {
        this.userRepository = userRepository;
        this.crmService = crmService;
    }

    public async Task<Result<long, Error>> Create(UserData model)
	{
		var nameResult = Name.Create(model.Name);
		var emailResult = Email.Create(model.Email);

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
            return Result.Failure<long, Error>(new Error(combinedErrorMessage));
        }

        User.Repository = this.userRepository; // Get user repository ready in User

		var user = User.Create(nameResult.Value, emailResult.Value);

        if (user.IsFailure)
        {
            return Result.Failure<long, Error>(user.Error);
        }

        await this.crmService.RegisterUser(user.Value.Name, user.Value.Email);

		return Result.Success<long, Error>(user.Value.Id);
	}
}