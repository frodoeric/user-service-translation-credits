using UserService.Application.Models;
using UserService.Infrastructure.Services;

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

        //public async Task AddCredits(UserData model)
        //{
        //    var user = userRepository.Get(model.Id);
        //    user.TranslationCredits.AddCredits(model.TranslationCredits);
        //    userRepository.Update(user);
        //    await crmService.UpdateUser(model);
        //}
    }
}
