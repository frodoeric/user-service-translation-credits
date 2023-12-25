using Moq;
using UserService.API.Contract.Users;
using UserService.Application.Models;
using UserService.Domain;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Services;

namespace UserService.Application.Tests.Unit
{
    public class UserCreatorTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<ICrmService> mockCrmService;
        private readonly UserCreator userCreator;

        public UserCreatorTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockCrmService = new Mock<ICrmService>();
            userCreator = new UserCreator(mockUserRepository.Object, mockCrmService.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnSuccess_WhenValidInput()
        {
            // Arrange
            var userData = new UserCreationRequest { Name = "John Doe", Email = "john@example.com" };
            mockUserRepository.Setup(repo => repo.GetAll()).Returns(new List<User>());

            // Act
            var result = await userCreator.Create(userData);

            // Assert
            Assert.True(result.IsSuccess);
            mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
            mockUserRepository.Verify(repo => repo.Save(), Times.Once);
            mockCrmService.Verify(crm => crm.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("Robert Lewandosky","invalidemail")]
        [InlineData("", "invalid@")]
        [InlineData("Robert", "@invalid.com")]
        [InlineData("", "valid@valid.com")]
        public async Task Create_ShouldReturnFailure_WhenInvalidInput(string name, string email)
        {
            // Arrange
            var userData = new UserCreationRequest { Name = name, Email = email };

            // Act
            var result = await userCreator.Create(userData);

            // Assert
            Assert.True(result.IsFailure);            
        }
    }

}