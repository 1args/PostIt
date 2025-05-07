using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Domain.ValueObjects;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Application.Validators.User;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("User name cannot be empty.")
            .MinimumLength(UserName.MinLength)
            .WithMessage($"User name must be at least {UserName.MinLength} characters long.")
            .MaximumLength(UserName.MaxLength)
            .WithMessage($"User name must be no longer than {UserName.MaxLength} characters long.");

        RuleFor(u => u.Bio)
            .MaximumLength(UserBio.MaxLength)
            .WithMessage($"User bio must be no longer than {UserBio.MaxLength} characters long.");

        RuleFor(u => u.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .Must(UserEmail.IsEmailValid).WithMessage("Invalid email format.");

        RuleFor(u => u.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(UserPassword.MinLength)
            .WithMessage($"Password must be at least {UserPassword.MinLength} characters long.")
            .MaximumLength(UserPassword.MaxLength)
            .WithMessage($"Password must be no longer than {UserPassword.MaxLength} characters long.")
            .Must(UserPassword.IsPasswordStrong)
            .WithMessage("Password must contain at least one upper case letter, one lower case letter, one digit, and one special character.");
    }
}