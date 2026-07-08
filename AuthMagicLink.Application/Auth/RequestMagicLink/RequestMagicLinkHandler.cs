using AuthMagicLink.Core.Entities;
using AuthMagicLink.Infra.Abstractions;

namespace AuthMagicLink.Application.Auth.RequestMagicLink;

public sealed class RequestMagicLinkHandler(
    IUserRepository userRepository,
    IMagicLinkRepository magicLinkRepository,
    ITokenGenerator tokenGenerator,
    ITokenHasher tokenHasher,
    IMagicLinkSender magicLinkSender,
    IUnitOfWork unitOfWork)
{
    public async Task<RequestMagicLinkResponse> HandleAsync(RequestMagicLinkRequest request,  CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        
        var user = await userRepository.GetByEmailAsync(
            normalizedEmail,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            return new RequestMagicLinkResponse(
                "Se o e-mail estiver cadastrado, enviaremos um link de acesso.");
        }

        var rawToken = tokenGenerator.Generate();
        var tokenHash = tokenHasher.Hash(rawToken);

        var magicLink = new MagicLink(
            user.Id,
            tokenHash,
            DateTime.UtcNow.AddMinutes(15));

        await magicLinkRepository.AddAsync(
            magicLink,
            cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        var magicLinkUrl = $"http://localhost:5173/auth/magic-link?token={rawToken}";

        await magicLinkSender.SendAsync(
            user.Email,
            magicLinkUrl,
            cancellationToken);

        return new RequestMagicLinkResponse(
            "Se o e-mail estiver cadastrado, enviaremos um link de acesso.");
    }
}