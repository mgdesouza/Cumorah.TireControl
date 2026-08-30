using System.ComponentModel.DataAnnotations;

namespace TireControl.Api.Contracts.Auth;

public sealed class LoginRequest
{
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(200, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}
