using FluentValidation;
using PostIt.Contracts.Requests.Post;

namespace PostIt.Application.Validators.Post;

/// <summary>
/// Data validator to change post visibility request.
/// </summary>
public class ChangePostVisibilityRequestValidator : AbstractValidator<ChangePostVisibilityRequest>
{
    public ChangePostVisibilityRequestValidator()
    {
        RuleFor(u => u.Visibility)
            .IsInEnum()
            .WithMessage("Visibility must be either Public or Private.");
    }
}