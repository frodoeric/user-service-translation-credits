using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects
{
    public class TranslationCredits : ValueObject<int>
    {
        public TranslationCredits(int value) : base(value)
        {
        }

        public Result<TranslationCredits, Error> SpendCredits(int credits)
        {
            if (!IsGreaterThanZero(credits))
            {
                return GreaterThanZeroError();
            }

            if (!IsInsufficentCredits(credits))
            {
                return InsufficentCreditsError();
            }

            Value -= credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        public Result<TranslationCredits, Error> SubtractCredits(int credits)
        {
            if (!IsGreaterThanZero(credits))
            {
                return GreaterThanZeroError();
            }

            Value -= credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        public Result<TranslationCredits, Error> AddCredits(int credits)
        {
            if (!IsGreaterThanZero(credits))
            {
                return GreaterThanZeroError();
            }

            Value += credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        private Result<TranslationCredits, Error> GreaterThanZeroError()
        {
            return Result.Failure<TranslationCredits, Error>(
            new TranslationCreditError(
                "Credits must be greater than 0.", nameof(TranslationCredits)));
        }

        private Result<TranslationCredits, Error> InsufficentCreditsError()
        {
            return Result.Failure<TranslationCredits, Error>(
            new TranslationCreditError(
                "Insufficient credits.", nameof(TranslationCredits)));
        }

        private static bool IsGreaterThanZero(int credits)
        {
            return credits > 0;
        }

        private bool IsInsufficentCredits(int credits)
        {
            return credits <= Value;
        }
    }
}