using System.ComponentModel.DataAnnotations;

namespace PostIt.Application.Contracts.Requests.User;

public record UpdateUserBioRequest(
    [Required] Guid UserId,
    [Required] string Bio);