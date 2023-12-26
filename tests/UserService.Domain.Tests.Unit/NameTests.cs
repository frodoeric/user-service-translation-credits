using UserService.Domain.ValueObjects;

namespace UserService.Domain.Test.Unit
{
    public class NameTests
	{
		[Fact]
		public void CanBeCreated()
		{
			// Arrange, Act
			var result = Name.Create("Robert Lewandosky");

			// Assert
			result.Should().BeSuccess();
		}

		[Fact]
		public void CannotBeEmpty()
		{
			// Arrange, Act
			var result = Name.Create("");

			// Assert
			result.Should().BeFailure();
		}

		[Fact]
		public void CannotBeLargerThan100Chars()
		{
			// Arrange
			var name = string.Concat(Enumerable.Repeat("a", 101));

			// Act
			var result = Name.Create(name);

			// Assert
			result.Should().BeFailure();
		}
	}
}