public record RefreshToken(
    int Id,
    int UserId,
    string Token,
    DateTime ExpiryDt,
    DateTime CreatedDt,
    string CreatedByIp
) : Identifiable
{
    #region Optional

    public DateTime? RevokedDt { get; set; } = default!;
    public string? RevokedByIp { get; set; } = default!;
    public string? ReplacedByToken { get; set; } = default!;
    public string? RevokedReason { get; set; } = default!;

    #endregion

    #region Calculated

    public bool IsExpired => DateTime.UtcNow >= ExpiryDt;
    public bool IsRevoked => RevokedDt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    #endregion
}