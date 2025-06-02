using FluentValidation;
using PostIt.Contracts.Requests.User;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Validators.User;

/// <summary>
/// Data validator to update user's biography request.
/// </summary>
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