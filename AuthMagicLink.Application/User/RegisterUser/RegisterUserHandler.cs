using AuthMagicLink.Application.User.RegisterUser;
using AuthMagicLink.Infra.Abstractions;

namespace AuthMagicLink.Application.User;

public sealed class RegisterUserHandler (IUnitOfWork unitOfWork, IUserRepository userRepository)
{
    
    public async Task<RegisterUserResponse> HandleAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existingUser = await userRepository.GetByEmailAsync(
            normalizedEmail,
            cancellationToken);

        if (existingUser is not null)
            throw new InvalidOperationException("Já existe um usuário com esse e-mail.");

        var user = new Core.Entities.User(request.Email, request.Name);

        await userRepository.AddAsync(user, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return new RegisterUserResponse(
            user.Id,
            user.Name,
            user.Email);
    }
}