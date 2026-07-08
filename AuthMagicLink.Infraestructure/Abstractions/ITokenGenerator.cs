using AuthMagicLink.Core.Entities;

namespace AuthMagicLink.Infra.Abstractions;

public interface ITokenGenerator
{
    string GenerateToken();
}