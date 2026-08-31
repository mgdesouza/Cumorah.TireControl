namespace TireControl.Api.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer) || string.IsNullOrWhiteSpace(Audience))
        {
            throw new InvalidOperationException("JWT:Issuer e JWT:Audience devem ser configurados.");
        }

        if (Key.Length < 32)
        {
            throw new InvalidOperationException("JWT:Key deve ter pelo menos 32 caracteres e ser fornecida por uma variável de ambiente ou cofre de segredos.");
        }

        if (ExpirationMinutes is < 1 or > 1440)
        {
            throw new InvalidOperationException("JWT:ExpirationMinutes deve estar entre 1 e 1440.");
        }
    }
}
