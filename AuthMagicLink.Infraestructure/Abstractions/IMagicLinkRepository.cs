using AuthMagicLink.Core.Entities;

namespace AuthMagicLink.Infra.Abstractions;

public interface IMagicLinkRepository
{
    Task<MagicLink?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);

    Task AddAsync(MagicLink magicLink, CancellationToken cancellationToken = default);
}