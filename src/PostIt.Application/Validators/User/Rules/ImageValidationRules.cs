using Microsoft.AspNetCore.Http;

namespace PostIt.Application.Validators.User.Rules;

/// <summary>
/// A set of rules for image validation.
/// </summary>
public static class ImageValidationRules
{
    /// <summary>
    ///  Checks if the file has a valid image format.
    /// </summary>
    /// <param name="file">File.</param>
    /// <returns></returns>
    public static bool BeAValidImage(IFormFile file)
    {
        var allowedFormats = new[] { "image/png", "image/jpeg", "image/jpg" };
        
        return allowedFormats.Contains(file.ContentType.ToLower());
    }

    /// <summary>
    /// Checks if the file size does not exceed the set limit.
    /// </summary>
    /// <param name="file">File</param>
    /// <returns></returns>
    public static bool BeWithinSizeLimit(IFormFile file)
    {
        const int maxSizeInBytes = 2 * 1024 * 1024;
        
        return file.Length <= maxSizeInBytes;
    }
}