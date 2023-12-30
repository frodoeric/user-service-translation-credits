using UserService.API.Contract.Users;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.Shared
{
    public class TranslationCreditsRequestBuilder
    {
        private int credits = 0;

        public TranslationCreditsRequest Build() => new()
        {
            TranslationCredits = credits,
        };

        public TranslationCreditsRequestBuilder WithCredits(int val)
        {
            credits = val;
            return this;
        }
    }
}
