using CSharpFunctionalExtensions;
using UserService.Domain.Core;

namespace UserService.Domain.ValueObjects;

public class Name : ValueObject<string>
{
    protected Name(string value) : base(value) { }

    public static Result<Name, Error> Set(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name, Error>(new Error("Name can't be empty"));

        if (name.Length > 100)
            return Result.Failure<Name, Error>(new Error("Name is too long"));

        return Result.Success<Name,Error>(new Name(name));
    }
}