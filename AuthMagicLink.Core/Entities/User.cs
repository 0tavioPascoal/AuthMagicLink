namespace AuthMagicLink.Core.Entities;

public sealed class User : EntityBase
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    
    private User() { }

    public User(string email, string name)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-mail é obrigatório.", nameof(email));
        
        if (!email.Contains('@'))
            throw new ArgumentException("E-mail inválido.", nameof(email));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório.", nameof(name));

        Email = email.Trim().ToLowerInvariant();
        Name = name.Trim();
    }

    public void Activate() => IsActive = true;
    
    public void Deactivate() => IsActive = false;
}