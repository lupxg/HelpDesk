namespace HelpDesk.Infrastructure.Authentication;

public sealed record JwtSettings(
    string Key, 
    string Issuer, 
    string Audience, 
    double DurationInMinutes);
