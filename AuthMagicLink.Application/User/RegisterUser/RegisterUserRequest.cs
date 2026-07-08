namespace AuthMagicLink.Application.User.RegisterUser;

public sealed record RegisterUserRequest(
    string Email,
    string Name);