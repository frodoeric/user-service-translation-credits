using UserService.API.Contract.Users;

namespace UserService.Tests.Shared
{
	public class UserUpdateRequestBuilder
	{
		private string email = UniqueEmailGenerator.Generate();
		private string name = "TestyModified";

		public UserUpdateRequest Build() => new()
		{
			Email = email,
			Name = name
		};

		public UserUpdateRequestBuilder WithEmail(string val)
		{
			email = val;
			return this;
		}

		public UserUpdateRequestBuilder WithName(string val)
		{
			name = val;
			return this;
		}
	}
}