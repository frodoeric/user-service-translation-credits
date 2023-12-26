using UserService.Application.Models;

namespace UserService.API.Contract.Users
{
    public class TranslationCreditsRequest : UserData
    {
        public int Credits { get; set; }
    }
}
