namespace AuthMagicLink.Core.Entities;

public sealed class MagicLink : EntityBase
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? ConsumedAtUtc { get; private set; }
    public bool IsConsumed  =>  ConsumedAtUtc != null;
    
    private MagicLink() { }

    public MagicLink(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
        {
            throw new ArgumentException("Hash do token é obrigatório.", nameof(tokenHash));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Usuário é obrigatório.", nameof(userId));
        }

        if (expiresAtUtc <= DateTime.UtcNow)
        {
            throw new ArgumentException("A data de expiração deve ser futura.", nameof(expiresAtUtc));
        }
        
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
    }

    public void Consume()
    {
        if (IsConsumed)
        {
            throw new InvalidOperationException("MagicLink já consumido");
        }
        
        if (DateTime.UtcNow > ExpiresAtUtc)
            throw new InvalidOperationException("Magic link expirado.");
        ConsumedAtUtc = DateTime.UtcNow;
    }
    
}