namespace AuthMagicLink.Application.Auth.VerifyMagicLink;

public record VerifyMagicLinkResponse(string AccessToken,
    Guid UserId,
    string Name,
    string Email);
