using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Tests.Unit
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldCreateUser_WhenValidData()
        {
            var result = User.Create("John Doe", "john@example.com");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("John Doe", result.Value.Name.Value);
            Assert.Equal("john@example.com", result.Value.Email.Value);
        }

        [Theory]
        [InlineData("", "email@example.com")]
        [InlineData("John Doe", "")]
        public void CreateUser_ShouldFail_WhenInvalidData(string name, string email)
        {
            var result = User.Create(name, email);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void UpdateName_ShouldUpdateName_WhenValidNewName()
        {
            var user = new User(Name.Create("John Doe").Value, Email.Create("john@example.com").Value);
            var newName = Name.Create("Jane Doe").Value;

            var result = user.UpdateName(newName);

            Assert.True(result.IsSuccess);
            Assert.Equal("Jane Doe", user.Name.Value);
        }

        [Fact]
        public void UpdateEmail_ShouldUpdateEmail_WhenValidNewEmail()
        {
            var user = new User(Name.Create("John Doe").Value, Email.Create("john@example.com").Value);
            var newEmail = Email.Create("jane@example.com").Value;

            var result = user.UpdateEmail(newEmail);

            Assert.True(result.IsSuccess);
            Assert.Equal("jane@example.com", user.Email.Value);
        }

        [Fact]
        public void AddCredits_ShouldIncreaseCredits_WhenPositiveAmount()
        {
            var user = new User(Name.Create("John Doe").Value, Email.Create("john@example.com").Value);

            var result = user.AddCredits(10);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, user.TranslationCredits.Value);
        }

        [Fact]
        public void SubtractCredits_ShouldDecreaseCredits_WhenSufficientCredits()
        {
            var user = new User(Name.Create("John Doe").Value, Email.Create("john@example.com").Value);
            user.AddCredits(15);

            var result = user.SubtractCredits(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, user.TranslationCredits.Value);
        }

    }
}
