using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Application.Validators.User;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(u => u.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .Must(Email.IsEmailValid).WithMessage("Invalid email format.");

        RuleFor(u => u.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(Password.MinLength)
            .WithMessage($"Password must be at least {Password.MinLength} characters long.")
            .MaximumLength(Password.MaxLength)
            .WithMessage($"Password must be no longer than {Password.MaxLength} characters long.");
    }
}