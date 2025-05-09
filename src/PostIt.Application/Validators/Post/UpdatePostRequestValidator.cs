using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Validators.Post;

public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(p => p.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Post title cannot be empty")
            .MinimumLength(PostTitle.MinLength)
            .WithMessage($"Title must be at least {PostTitle.MinLength} characters long.")
            .MaximumLength(PostTitle.MaxLength)
            .WithMessage($"Title name must be no longer than {PostTitle.MaxLength} characters long.");

        RuleFor(p => p.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Content cannot be empty")
            .MinimumLength(PostContent.MinLength)
            .WithMessage($"Content must be at least {PostContent.MinLength} characters long.")
            .MaximumLength(PostContent.MaxLength)
            .WithMessage($"Content must be no longer than {PostContent.MaxLength} characters long.");
    }
}