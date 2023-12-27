using UserService.Domain.Core;
using UserService.Infrastructure.Services;
using UserService.Domain.ValueObjects;

namespace UserService.Application
{
    public class UserCreditsService
    {
        private readonly IUserRepository userRepository;
        private readonly ICrmService crmService;

        public UserCreditsService(IUserRepository userRepository, ICrmService crmService)
        {
            this.userRepository = userRepository;
            this.crmService = crmService;
        }

        public async Task<Result<TranslationCredits, Error>> AddCredits(long userId, int credits) =>
            await UpdateUserCredits(userId, user => user.AddCredits(credits));

        public async Task<Result<TranslationCredits, Error>> SpendCredits(long userId, int credits) =>
            await UpdateUserCredits(userId, user => user.SpendCredits(credits));

        public async Task<Result<TranslationCredits, Error>> SubtractCredits(long userId, int credits) =>
            await UpdateUserCredits(userId, user => user.SubtractCredits(credits));

        private async Task<Result<TranslationCredits, Error>> UpdateUserCredits(long userId, Func<User, Result<User, Error>> operation)
        {
            var user = userRepository.Get(userId);
            if (user == null)
                return Result.Failure<TranslationCredits, Error>(new Error("User not found"));

            var result = operation(user);
            if (result.IsFailure)
                return Result.Failure<TranslationCredits, Error>(result.Error);

            userRepository.Update(user);
            await crmService.UpdateUser(user);
            userRepository.Save();
            return Result.Success<TranslationCredits, Error>(user.TranslationCredits);
        }
    }
}
