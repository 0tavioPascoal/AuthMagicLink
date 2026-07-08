namespace AuthMagicLink.Infra.Abstractions;

public interface ITokenHasher
{
    string Hash(string token);
}