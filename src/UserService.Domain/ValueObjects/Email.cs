using System.Text.RegularExpressions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects;

public class Email : ValueObject<string>
{
    protected Email(string value) : base(value) { }

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return EmailEmptyError();

        if (GreaterThen100(email))
            return EmailTooLongError();

        if (!EmailIsValid(email))
            return Result.Failure<Email, Error>(new Error("Email is invalid"));

        return Result.Success<Email, Error>(new Email(email));
    }

    private static bool EmailIsValid(string email)
    {
        return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    }

    private static bool GreaterThen100(string email)
    {
        return email.Length > 100;
    }

    private static Result<Email, Error> EmailEmptyError()
    {
        return Result.Failure<Email, Error>(
        new UserValidationError(
            "Email can't be empty.", nameof(Email)));
    }

    private static Result<Email, Error> EmailTooLongError()
    {
        return Result.Failure<Email, Error>(
        new UserValidationError(
            "Email is too long.", nameof(Name)));
    }
}