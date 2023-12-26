using FluentValidation;

namespace UserService.API.Contract.Users.Validator
{
    public class UserCreationRequestValidator : AbstractValidator<UserCreationRequest>
    {
        public UserCreationRequestValidator()
        {
            RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters");
        }
    }
}
