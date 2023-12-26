using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Contract.Users;
using UserService.Domain;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Services;

namespace UserService.Application.Tests.Unit
{
    public class UserCreditsServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<ICrmService> mockCrmService;
        private readonly UserCreditsService userCreditsService;

        public UserCreditsServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockCrmService = new Mock<ICrmService>();
            userCreditsService = new UserCreditsService(mockUserRepository.Object, mockCrmService.Object);
        }

        [Fact]
        public async Task AddCredits_ShouldReturnSuccess_WhenUserExists()
        {
            var oldUser = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns(oldUser);

            var creditsToAdd = 10;
            var result = await userCreditsService.AddCredits(1, creditsToAdd);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, oldUser.TranslationCredits.Value);
            mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            mockCrmService.Verify(crm => crm.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AddCredits_ShouldReturnSuccess_WhenUserExistsAndCreditsAreAdded()
        {
            var oldUser = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            var request = new TranslationCreditsRequest() { Credits = 10 };
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns(oldUser);

            var result = await userCreditsService.AddCredits(1, 10);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, oldUser.TranslationCredits.Value);
            mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            mockCrmService.Verify(crm => crm.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AddCredits_ShouldReturnFailure_WhenUserNotFound()
        {
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns((User)null);

            var result = await userCreditsService.AddCredits(1, 10);

            Assert.True(result.IsFailure);
            Assert.Equal("User not found", result.Error.Message);
        }

        [Fact]
        public async Task SubtractCredits_ShouldReturnSuccess_WhenSufficientCredits()
        {
            var oldUser = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            oldUser.AddCredits(15);
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns(oldUser);

            var creditsToSubtract = 5;
            var result = await userCreditsService.SubtractCredits(1, creditsToSubtract);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, oldUser.TranslationCredits.Value);
            mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            mockCrmService.Verify(crm => crm.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task SubtractCredits_ShouldReturnSuccess_WhenUserExistsAndCreditsAreSubtracted()
        {
            var oldUser = new User(Name.Create("Robert Lewandosky").Value, Email.Create("test@example.com").Value);
            oldUser.AddCredits(15);
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns(oldUser);

            var result = await userCreditsService.SubtractCredits(1, 5);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, oldUser.TranslationCredits.Value);
        }

        [Fact]
        public async Task SubtractCredits_ShouldReturnFailure_WhenUserNotFound()
        {
            mockUserRepository.Setup(repo => repo.Get(It.IsAny<long>())).Returns((User)null);

            var result = await userCreditsService.SubtractCredits(1, 5);

            Assert.True(result.IsFailure);
            Assert.Equal("User not found", result.Error.Message);
        }

    }
}
