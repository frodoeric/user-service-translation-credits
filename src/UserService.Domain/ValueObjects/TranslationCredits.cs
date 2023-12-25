
using CSharpFunctionalExtensions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects
{
    public class TranslationCredits : ValueObject<int>
    {
        public TranslationCredits(int value) : base(value)
        {
        }

        public Result<TranslationCredits, Error> SubtractCredits(int credits)
        {
            if (credits <= 0)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Credits must be greater than 0"));
            }

            if (Value < credits)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Insufficient credits"));
            }

            Value -= credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        public Result<TranslationCredits, Error> AddCredits(int credits)
        {
            if (credits <= 0)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Credits must be greater than 0"));
            }

            Value += credits;
            return Result.Success<TranslationCredits, Error>(this);
        }
    }
}