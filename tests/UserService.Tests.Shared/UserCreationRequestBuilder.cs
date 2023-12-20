using UserService.API.Contract.Users;

namespace UserService.Tests.Shared
{
	public class UserCreationRequestBuilder
	{
		private string email = UniqueEmailGenerator.Generate();
		private string name = "Testy";

		public UserCreationRequest Build() => new()
		{
			Email = email,
			Name = name
		};

		public UserCreationRequestBuilder WithEmail(string val)
		{
			email = val;
			return this;
		}

		public UserCreationRequestBuilder WithName(string val)
		{
			name = val;
			return this;
		}
	}
}