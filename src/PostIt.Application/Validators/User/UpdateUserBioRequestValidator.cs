using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Application.Validators.User;

public class UpdateUserBioRequestValidator : AbstractValidator<UpdateUserBioRequest>
{
    public UpdateUserBioRequestValidator()
    {
        RuleFor(u => u.Bio)
            .NotEmpty().WithMessage("Bio cannot be empty.")
            .MaximumLength(UserBio.MaxLength)
            .WithMessage($"User bio must be no longer than {UserBio.MaxLength} characters long.");
    }
}