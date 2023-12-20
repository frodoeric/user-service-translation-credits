namespace UserService.Domain;

public class Name : ValueObject<string>
{
	protected Name(string value) : base(value) { }

	public static Result<Name> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<Name>("Name can't be empty");

		if (name.Length > 100)
			return Result.Failure<Name>("Name is too long");

		return Result.Success(new Name(name));
	}
}