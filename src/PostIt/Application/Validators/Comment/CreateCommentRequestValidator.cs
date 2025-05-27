using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Validators.Comment;

/// <summary>
/// Data validator for comment creation request.
/// </summary>
public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(c => c.Text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Text cannot be empty.")
            .MinimumLength(CommentText.MinLength)
            .WithMessage($"Text must be at least {CommentText.MinLength} characters long.")
            .MaximumLength(CommentText.MaxLength)
            .WithMessage($"Text must be no longer than {CommentText.MaxLength} characters long.");

        RuleFor(c => c.PostId)
            .NotEmpty().WithMessage("Post ID cannot be empty.");
    }
}