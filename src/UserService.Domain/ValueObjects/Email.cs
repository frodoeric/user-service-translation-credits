namespace UserService.Domain.ValueObjects;

public class Email : ValueObject<string>
{
    protected Email(string value) : base(value) { }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>("Email can't be empty");

        if (email.Length > 100)
            return Result.Failure<Email>("Email is too long");

        return Result.Success(new Email(email));
    }
}