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

        public async Task<Result<TranslationCredits, Error>> AddCredits(long userId, int credits)
        {
            var user = userRepository.Get(userId);
            if (user == null)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("User not found"));
            }

            var result = user.AddCredits(credits);
            if (result.IsFailure)
            {
                return Result.Failure<TranslationCredits, Error>(result.Error);
            }

            userRepository.Update(user);
            await crmService.UpdateUser(user);
            return Result.Success<TranslationCredits, Error>(user.TranslationCredits);
        }

        public async Task<Result<TranslationCredits, Error>> SubtractCredits(long userId, int credits)
        {
            var user = userRepository.Get(userId);
            if (user == null)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("User not found"));
            }

            var result = user.TranslationCredits.SubtractCredits(credits);
            if (result.IsFailure)
            {
                return Result.Failure<TranslationCredits, Error>(result.Error);
            }

            userRepository.Update(user);
            await crmService.UpdateUser(user);
            userRepository.Save();
            return Result.Success<TranslationCredits, Error>(user.TranslationCredits);
        }
    }
}
