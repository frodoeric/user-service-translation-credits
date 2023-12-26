using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Tests.Unit
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldCreateUser_WhenValidData()
        {
            var result = User.Create("Robert Lewandosky", "test@example.com");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Robert Lewandosky", result.Value.Name.Value);
            Assert.Equal("test@example.com", result.Value.Email.Value);
        }

        [Theory]
        [InlineData("", "email@example.com")]
        [InlineData("Robert Lewandosky", "")]
        public void CreateUser_ShouldFail_WhenInvalidData(string name, string email)
        {
            var result = User.Create(name, email);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void UpdateName_ShouldUpdateName_WhenValidNewName()
        {
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            var newName = Name.Create("Jane Doe").Value;

            var result = user.UpdateName(newName);

            Assert.True(result.IsSuccess);
            Assert.Equal("Jane Doe", user.Name.Value);
        }

        [Fact]
        public void UpdateEmail_ShouldUpdateEmail_WhenValidNewEmail()
        {
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            var newEmail = Email.Create("jane@example.com").Value;

            var result = user.UpdateEmail(newEmail);

            Assert.True(result.IsSuccess);
            Assert.Equal("jane@example.com", user.Email.Value);
        }

        [Fact]
        public void AddCredits_ShouldIncreaseCredits_WhenPositiveAmount()
        {
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);

            var result = user.AddCredits(10);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, user.TranslationCredits.Value);
        }

        [Fact]
        public void SubtractCredits_ShouldDecreaseCredits_WhenSufficientCredits()
        {
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            user.AddCredits(15);

            var result = user.SubtractCredits(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, user.TranslationCredits.Value);
        }

        [Theory]
        [InlineData(0, UserTier.Sporadic)]
        [InlineData(50, UserTier.Sporadic)]
        [InlineData(100, UserTier.Advanced)]
        [InlineData(500, UserTier.Advanced)]
        [InlineData(1000, UserTier.Special)]
        [InlineData(2000, UserTier.Special)]
        public void UserTier_ShouldBeCorrectBasedOnTotalCreditsSpent(int totalSpent, UserTier expectedTier)
        {
            // Arrange
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);

            // Act
            user.AddCredits(totalSpent);
            user.SpendCredits(totalSpent);

            // Assert
            Assert.Equal(expectedTier, user.Tier);
        }

        [Fact]
        public void AddCredits_ShouldIncreaseTotalCreditsSpent_WhenCreditsAdded()
        {
            // Arrange
            var user = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);

            // Act
            user.AddCredits(10);
            user.SpendCredits(10);

            // Assert
            Assert.Equal(10, user.TotalCreditsSpent);
        }
    }
}
