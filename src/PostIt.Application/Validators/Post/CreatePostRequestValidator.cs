using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Domain.ValueObjects.Post;

namespace PostIt.Application.Validators.Post;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(p => p.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Post title cannot be empty")
            .MinimumLength(Title.MinLength)
            .WithMessage($"Title must be at least {Title.MinLength} characters long.")
            .MaximumLength(Title.MaxLength)
            .WithMessage($"Title must be no longer than {Title.MaxLength} characters long.");

        RuleFor(p => p.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Content cannot be empty")
            .MinimumLength(Content.MinLength)
            .WithMessage($"Content must be at least {Content.MinLength} characters long.")
            .MaximumLength(Content.MaxLength)
            .WithMessage($"Content must be no longer than {Content.MaxLength} characters long.");
        
        RuleFor(u => u.Visibility)
            .IsInEnum()
            .WithMessage("Visibility must be either Public or Private.");
    }
}