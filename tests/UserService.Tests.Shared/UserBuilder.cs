using UserService.Domain.ValueObjects;

namespace UserService.Tests.Shared
{
    public class UserBuilder
	{
        private Email email = Email.Create(UniqueEmailGenerator.Generate()).Value;
		private Name name = Name.Create("Testy").Value;

		public User Build() => new(name, email);

		public UserBuilder WithEmail(Email val)
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