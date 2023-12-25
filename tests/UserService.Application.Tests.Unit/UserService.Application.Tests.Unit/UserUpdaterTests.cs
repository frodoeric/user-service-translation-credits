using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Models;
using UserService.Domain.ValueObjects;
using UserService.Domain;
using UserService.Infrastructure.Services;
using UserService.API.Contract.Users;

namespace UserService.Application.Tests.Unit
{
    public class UserUpdaterTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<ICrmService> mockCrmService;
        private readonly UserUpdater userUpdater;

        public UserUpdaterTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockCrmService = new Mock<ICrmService>();
            userUpdater = new UserUpdater(mockUserRepository.Object, mockCrmService.Object);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenValidInput()
        {
            // Arrange
            var userId = 1;
            var userRequest = new UserUpdateRequest { Name = "Robert Lewandosky", Email = "test@example.com" };

            var oldUser = new User(Name.Set("Robert Lewandosky").Value, Email.Set("test@example.com").Value);

            mockUserRepository.Setup(repo => repo.Get(userId)).Returns(oldUser);
            mockUserRepository.Setup(repo => repo.Update(oldUser));
            mockUserRepository.Setup(repo => repo.Save());

            // Act
            var result = await userUpdater.Update(userId, userRequest);

            // Assert
            Assert.True(result.IsSuccess);
            mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            mockUserRepository.Verify(repo => repo.Save(), Times.Once);
            mockCrmService.Verify(crm => crm.UpdateUser(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(1, "Robert Lewandosky", "invalidemail")]
        [InlineData(0, "", "invalid@")]
        [InlineData(0, "Robert", "")]
        [InlineData(1, "", "valid@valid.com")]
        public async Task Update_ShouldReturnFailure_WhenInvalidInput(long id, string name, string email)
        {
            // Arrange
            var userData = new UserUpdateRequest { Name = name, Email = email };
            var user = new User(Name.Set("Robert Lewandosky").Value, Email.Set("test@example.com").Value);

            mockUserRepository.Setup(repo => repo.Get(1)).Returns(user);
            mockUserRepository.Setup(repo => repo.Update(user));
            mockUserRepository.Setup(repo => repo.Save());

            // Act
            var result = await userUpdater.Update(id, userData);

            // Assert
            Assert.True(result.IsFailure);
        }
    }

}
