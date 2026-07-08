namespace AuthMagicLink.Infra.Abstractions;

public interface IMagicLinkSender
{
    Task SendAsync(string email, string magicLinkUrl, CancellationToken cancellationToken = default);
}