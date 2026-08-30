using TireControl.Domain.Entities;

namespace TireControl.Api.Authentication;

public interface ITokenService
{
    AuthToken Create(Usuario usuario, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions);
}

public sealed record AuthToken(string AccessToken, DateTime ExpiresAtUtc);
