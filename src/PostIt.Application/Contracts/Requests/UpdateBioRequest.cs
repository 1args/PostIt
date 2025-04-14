namespace PostIt.Application.Contracts.Requests;

public record UpdateBioRequest(
    Guid UserId,
    string Bio);