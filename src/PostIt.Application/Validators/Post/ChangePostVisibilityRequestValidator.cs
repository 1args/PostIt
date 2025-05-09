using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.Post;

namespace PostIt.Application.Validators.Post;

public class ChangePostVisibilityRequestValidator : AbstractValidator<ChangePostVisibilityRequest>
{
    public ChangePostVisibilityRequestValidator()
    {
        RuleFor(u => u.Visibility)
            .IsInEnum()
            .WithMessage("Visibility must be either Public or Private.");
    }
}