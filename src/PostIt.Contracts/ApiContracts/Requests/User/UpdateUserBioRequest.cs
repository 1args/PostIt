using System.ComponentModel.DataAnnotations;

namespace PostIt.Contracts.ApiContracts.Requests.User;

public record UpdateUserBioRequest(
    string Bio);