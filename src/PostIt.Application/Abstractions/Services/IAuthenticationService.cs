using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Services;

public interface IAuthenticationService
{
    string GenerateAccessToken(User user, IEnumerable<Claim>? additionalClaims = null);

    Task<string> GenerateAndStoreRefreshTokenAsync(User user, CancellationToken cancellationToken);

    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(User user,
        CancellationToken cancellationToken);

    Task<(string accessToken, string refreshToken)> RefreshAccessTokenAsync(CancellationToken cancellationToken);

    Task RevokeRefreshTokenAsync(CancellationToken cancellationToken);

    string? GetAccessTokenFromHeader();

    string? GetRefreshTokenFromHeader();

    Guid GetUserIdFromAccessToken();

    string GetUserRoleFromAccessToken();
}