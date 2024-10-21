namespace Sensify.Workers.Wanesy;

internal record LoginResponse(ulong ExpiredDate, string TokenType, string Token)
{
    public DateTime GetExpiredDate()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds((long)ExpiredDate).UtcDateTime;
    }
}
