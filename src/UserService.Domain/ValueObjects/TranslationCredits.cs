
using CSharpFunctionalExtensions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects
{
    public class TranslationCredits : ValueObject
    {
        public int Balance { get; private set; }

        public TranslationCredits(int balance)
        {
            Balance = balance;
        }

        public Result<TranslationCredits, Error> SubtractCredits(int credits)
        {
            if (credits <= 0)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Credits must be greater than 0"));
            }

            if (this.Balance < credits)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Insufficient credits"));
            }

            this.Balance -= credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        public Result<TranslationCredits, Error> AddCredits(int credits)
        {
            if (credits <= 0)
            {
                return Result.Failure<TranslationCredits, Error>(new Error("Credits must be greater than 0"));
            }

            this.Balance += credits;
            return Result.Success<TranslationCredits, Error>(this);
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Balance;
        }
    }
}