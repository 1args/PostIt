using FluentValidation;
using PostIt.Application.Validators.User.Rules;
using PostIt.Contracts.ApiContracts.Requests.User;

namespace PostIt.Application.Validators.User;

/// <summary>
/// Data validator for avatar upload request.
/// </summary>
public class UploadAvatarRequestValidator : AbstractValidator<UploadAvatarRequest>
{
    public UploadAvatarRequestValidator()
    {
        RuleFor(u => u.Avatar)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("An image file is required.")
            .Must(ImageValidationRules.BeAValidImage)
            .WithMessage("File must be an image (png, jpeg, jpg).")
            .Must(ImageValidationRules.BeWithinSizeLimit)
            .WithMessage("The file size should not exceed 2 MB.");
    }
}