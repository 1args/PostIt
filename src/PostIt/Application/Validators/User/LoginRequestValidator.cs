using FluentValidation;
using PostIt.Contracts.Requests.User;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Validators.User;

/// <summary>
/// Data validator for user log in request.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
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
            .WithMessage($"Password must be no longer than {UserPassword.MaxLength} characters long.");
    }
}