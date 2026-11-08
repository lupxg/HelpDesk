namespace HelpDesk.Application.Contracts.Auth;

public sealed record AuthenticationResponse(
    string Id, 
    string UserName, 
    string Email, 
    List<string> Roles, 
    bool IsVerified, 
    string AccessToken, 
    string RefreshToken);
