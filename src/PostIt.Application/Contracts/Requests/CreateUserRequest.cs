using System.ComponentModel.DataAnnotations;
using PostIt.Domain.Enums;

namespace PostIt.Application.Contracts.Requests;

public sealed record CreateUserRequest(
    [Required] string Name,
    string Bio,
    [Required] string Email,
    [Required] string Password,
    [Required] Role Role);
