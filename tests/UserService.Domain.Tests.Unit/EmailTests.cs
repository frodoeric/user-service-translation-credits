using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Tests.Unit
{
    public class EmailTests
    {
        [Fact]
        public void Create_ShouldReturnSuccess_WhenEmailIsValid()
        {
            // Arrange
            var validEmail = "test@example.com";

            // Act
            var result = Email.Set(validEmail);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validEmail, result.Value.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_ShouldReturnFailure_WhenEmailIsEmpty(string invalidEmail)
        {
            // Act
            var result = Email.Set(invalidEmail);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Email can't be empty", result.Error.Message);
        }

        [Fact]
        public void Create_ShouldReturnFailure_WhenEmailIsTooLong()
        {
            // Arrange
            var longEmail = new string('a', 101) + "@example.com";

            // Act
            var result = Email.Set(longEmail);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Email is too long", result.Error.Message);
        }

        [Theory]
        [InlineData("invalidemail")]
        [InlineData("invalid@")]
        [InlineData("@invalid.com")]
        public void Create_ShouldReturnFailure_WhenEmailIsInvalid(string invalidEmail)
        {
            // Act
            var result = Email.Set(invalidEmail);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Email is invalid", result.Error.Message);
        }
    }

}
