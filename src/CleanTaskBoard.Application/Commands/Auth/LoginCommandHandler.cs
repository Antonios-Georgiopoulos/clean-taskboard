using CleanTaskBoard.Application.Auth;
using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var valid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash,
            user.PasswordSalt
        );

        if (!valid)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Username, user.Email);

        return new AuthResult(user.Id, user.Username, user.Email, token);
    }
}
