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

    public async Task<Result<User, Error>> Update(long userId, UserData model)
    {
        var user = userRepository.Get(userId);
        if (user == null)
        {
            return Result.Failure<User, Error>(new Error("User not found"));
        }

        Result<User, Error> updateResult;

        if (model.Name != null)
        {
            var nameResult = Name.Set(model.Name);
            if (nameResult.IsFailure)
            {
                return Result.Failure<User, Error>(nameResult.Error);
            }

            updateResult = user.UpdateName(nameResult.Value);
            if (updateResult.IsFailure)
            {
                return Result.Failure<User, Error>(updateResult.Error);
            }
        }

        if (model.Email != null)
        {
            var emailResult = Email.Create(model.Email);
            if (emailResult.IsFailure)
            {
                return Result.Failure<User, Error>(emailResult.Error);
            }

            updateResult = user.UpdateEmail(emailResult.Value);
            if (updateResult.IsFailure)
            {
                return Result.Failure<User, Error>(updateResult.Error);
            }
        }

        userRepository.Update(user);
        userRepository.Save();

        if (model.Name != null || model.Email != null)
        {
            await crmService.UpdateUser(user);
        }

        return Result.Success<User, Error>(user);
    }
}

