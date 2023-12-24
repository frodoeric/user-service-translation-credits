using System.Text.RegularExpressions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects;

public class Email : ValueObject<string>
{
    protected Email(string value) : base(value) { }

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email, Error>(new Error("Email can't be empty"));

        if (email.Length > 100)
            return Result.Failure<Email, Error>(new Error("Email is too long"));

        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            return Result.Failure<Email, Error>(new Error("Email is invalid"));

        return Result.Success<Email, Error>(new Email(email));
    }
}