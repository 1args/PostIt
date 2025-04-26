using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Application.Validators.Comment;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage("Text cannot be empty.")
            .MinimumLength(Text.MinLength)
            .WithMessage($"Text must be at least {Text.MinLength} characters long.")
            .MaximumLength(Text.MaxLength)
            .WithMessage($"Text must be no longer than {Text.MaxLength} characters long.");

        RuleFor(c => c.AuthorId)
            .NotEmpty().WithMessage("Author ID cannot be empty.");

        RuleFor(c => c.PostId)
            .NotEmpty().WithMessage("Post ID cannot be empty.");
    }
}