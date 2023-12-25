using UserService.Application.Models;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Services;

namespace UserService.Application;
public class UserUpdater
{
    private readonly IUserRepository userRepository;
    private readonly ICrmService crmService;

    public UserUpdater(IUserRepository userRepository, ICrmService crmService)
    {
        this.userRepository = userRepository;
        this.crmService = crmService;
    }

    public async Task<Result<long, Error>> Update(long userId, UserData model)
    {
        var user = userRepository.Get(userId);
        if (user == null)
        {
            return Result.Failure<long, Error>(new Error("User not found"));
        }

        Result<User, Error> updateResult;

        User.Repository = this.userRepository;

        if (model.Name != null)
        {
            var nameResult = Name.Set(model.Name);
            if (nameResult.IsFailure)
            {
                return Result.Failure<long, Error>(nameResult.Error);
            }
        }

        if (model.Email != null)
        {
            var emailResult = Email.Set(model.Email);
            if (emailResult.IsFailure)
            {
                return Result.Failure<long, Error>(emailResult.Error);
            }
        }

        userRepository.Update(user);
        userRepository.Save();

        if (model.Name != null || model.Email != null)
        {
            await crmService.UpdateUser(user.Id, user.Name, user.Email);
        }

        return Result.Success<long, Error>(userId);
    }
}

