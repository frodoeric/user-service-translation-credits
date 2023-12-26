using FluentValidation;

namespace UserService.API.Contract.Users.Validator
{
    public class TranslationCreditsRequestValidator : AbstractValidator<TranslationCreditsRequest>
    {
        public TranslationCreditsRequestValidator()
        {
            RuleFor(request => request.Credits)
                .NotEmpty().WithMessage("Credits is required")
                .GreaterThan(0).WithMessage("Credits must be greater than 0");
        }
    }
}
