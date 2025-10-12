/// <summary>
/// Security settings stored in appsettings json file(s).
/// </summary>
public class SecuritySettings
{
    /// <summary>
    /// Secret used to hash passwords.
    /// </summary>
    public string Secret { get; set; } = default!;

    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    public int RefreshTokenTTL { get; set; }

}