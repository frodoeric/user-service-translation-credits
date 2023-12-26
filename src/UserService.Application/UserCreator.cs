using CSharpFunctionalExtensions;
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

    public async Task<Result<User, Error>> Create(UserData model)
	{
		var user = User.Create(name: model.Name, email: model.Email);

        if (user.IsFailure)
        {
            return Result.Failure<User, Error>(user.Error);
        }

        var allUsers = userRepository.GetAll();
        if (allUsers.Any(u => u.Email == model.Email))
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "User with given Email already exists.", nameof(User), nameof(User.Email)));

        if (allUsers.Any(u => u.Name == model.Name && u.Id != user.Value.Id))
        {
            return Result.Failure<User, Error>(
                new UniqueConstraintViolationError(
                    "Another user with the same Name already exists.", nameof(User), nameof(User.Name)));
        }

        userRepository.Add(user.Value);
        await this.crmService.RegisterUser(user.Value);
        userRepository.Save();

		return Result.Success<User, Error>(user.Value);
	}
}