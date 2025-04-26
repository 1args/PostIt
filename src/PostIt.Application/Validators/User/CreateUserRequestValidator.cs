using FluentValidation;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Application.Validators.User;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("User name cannot be empty.")
            .MinimumLength(Name.MinLength)
            .WithMessage($"User name must be at least {Name.MinLength} characters long.")
            .MaximumLength(Name.MaxLength)
            .WithMessage($"User name must be no longer than {Name.MaxLength} characters long.");

        RuleFor(u => u.Bio)
            .MaximumLength(Bio.MaxLength)
            .WithMessage($"User bio must be no longer than {Bio.MaxLength} characters long.");

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
            .WithMessage($"Password must be no longer than {Password.MaxLength} characters long.")
            .Must(Password.IsPasswordStrong)
            .WithMessage("Password must contain at least one upper case letter, one lower case letter, one digit, and one special character.");
    }
}