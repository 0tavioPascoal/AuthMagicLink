using AuthMagicLink.Core.Entities;

namespace AuthMagicLink.Infra.Abstractions;

public interface IJwtTokenProvider
{
    string Generate(User user);
}