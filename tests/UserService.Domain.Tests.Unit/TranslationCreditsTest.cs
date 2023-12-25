using UserService.Domain.ValueObjects;

namespace UserService.Domain.Tests.Unit
{
    public class TranslationCreditsTests
    {
        [Fact]
        public void AddCredits_ShouldIncreaseCredits_WhenPositiveAmount()
        {
            var credits = new TranslationCredits(10);
            var result = credits.AddCredits(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(15, credits.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddCredits_ShouldFail_WhenNonPositiveAmount(int amount)
        {
            var credits = new TranslationCredits(10);
            var result = credits.AddCredits(amount);

            Assert.True(result.IsFailure);
            Assert.Equal("Credits must be greater than 0", result.Error.Message);
        }

        [Fact]
        public void SubtractCredits_ShouldDecreaseCredits_WhenAmountIsAvailable()
        {
            var credits = new TranslationCredits(10);
            var result = credits.SubtractCredits(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(5, credits.Value);
        }

        [Fact]
        public void SubtractCredits_ShouldFail_WhenInsufficientCredits()
        {
            var credits = new TranslationCredits(5);
            var result = credits.SubtractCredits(10);

            Assert.True(result.IsFailure);
            Assert.Equal("Insufficient credits", result.Error.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SubtractCredits_ShouldFail_WhenNonPositiveAmount(int amount)
        {
            var credits = new TranslationCredits(10);
            var result = credits.SubtractCredits(amount);

            Assert.True(result.IsFailure);
            Assert.Equal("Credits must be greater than 0", result.Error.Message);
        }
    }

}
