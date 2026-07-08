namespace AuthMagicLink.Application.User.RegisterUser;

public record RegisterUserResponse(
    Guid UserId,
    string Email,
    string Name);