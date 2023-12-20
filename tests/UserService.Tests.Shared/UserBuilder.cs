namespace UserService.Tests.Shared
{
	public class UserBuilder
	{
		private string email = UniqueEmailGenerator.Generate();
		private Name name = Name.Create("Testy").Value;

		public User Build() => new(name, email);

		public UserBuilder WithEmail(string val)
		{
			email = val;
			return this;
		}

		public UserBuilder WithName(Name val)
		{
			name = val;
			return this;
		}
	}
}