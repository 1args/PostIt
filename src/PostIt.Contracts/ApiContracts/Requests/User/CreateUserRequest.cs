using System.ComponentModel.DataAnnotations;
using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.User;

public sealed record CreateUserRequest(
    [Required] string Name,
    [Required] string Bio,
    [Required] string Email,
    [Required] string Password,
    [Required] Role Role);
