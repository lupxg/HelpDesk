namespace HelpDesk.Application.Contracts.Auth;

public sealed record RegistrationRequest(
    string Email, 
    string Password);
