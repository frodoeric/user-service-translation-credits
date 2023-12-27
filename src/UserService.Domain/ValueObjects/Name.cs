using CSharpFunctionalExtensions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects;

public class Name : ValueObject<string>
{
    protected Name(string value) : base(value) { }

    public static implicit operator string(Name name) => name.Value;

    public static Result<Name, Error> Set(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return NameEmptyError();

        if (GreaterThen100(name))
            return NameTooLongError();

        return Result.Success<Name, Error>(new Name(name));
    }

    private static bool GreaterThen100(string name)
    {
        return name.Length > 100;
    }

    private static Result<Name, Error> NameEmptyError()
    {
        return Result.Failure<Name, Error>(
        new UserValidationError(
            "Name can't be empty.", nameof(Name)));
    }

    private static Result<Name, Error> NameTooLongError()
    {
        return Result.Failure<Name, Error>(
        new UserValidationError(
            "Name is too long.", nameof(Name)));
    }
}