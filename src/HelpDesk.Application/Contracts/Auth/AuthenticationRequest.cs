namespace HelpDesk.Application.Contracts.Auth;

public sealed record AuthenticationRequest(
    string Email, 
    string Password);
