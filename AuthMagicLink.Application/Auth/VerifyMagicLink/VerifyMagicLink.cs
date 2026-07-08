using AuthMagicLink.Infra.Abstractions;

namespace AuthMagicLink.Application.Auth.VerifyMagicLink;

public sealed class VerifyMagicLinkHandler
{
    private readonly IMagicLinkRepository _magicLinkRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenHasher _tokenHasher;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyMagicLinkHandler(
        IMagicLinkRepository magicLinkRepository,
        IUserRepository userRepository,
        ITokenHasher tokenHasher,
        IJwtTokenProvider jwtTokenProvider,
        IUnitOfWork unitOfWork)
    {
        _magicLinkRepository = magicLinkRepository;
        _userRepository = userRepository;
        _tokenHasher = tokenHasher;
        _jwtTokenProvider = jwtTokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<VerifyMagicLinkResponse> HandleAsync(
        VerifyMagicLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            throw new InvalidOperationException("Token é obrigatório.");

        var tokenHash = _tokenHasher.Hash(request.Token);

        var magicLink = await _magicLinkRepository.GetByTokenHashAsync(
            tokenHash,
            cancellationToken);

        if (magicLink is null)
            throw new InvalidOperationException("Magic link inválido.");

        magicLink.Consume();

        var user = await _userRepository.GetByIdAsync(
            magicLink.UserId,
            cancellationToken);

        if (user is null || !user.IsActive)
            throw new InvalidOperationException("Usuário inválido ou inativo.");

        var accessToken = _jwtTokenProvider.Generate(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new VerifyMagicLinkResponse(
            accessToken,
            user.Id,
            user.Name,
            user.Email);
    }
}