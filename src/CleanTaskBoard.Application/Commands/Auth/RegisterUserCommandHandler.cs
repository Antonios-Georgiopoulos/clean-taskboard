using CleanTaskBoard.Application.Auth;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Users;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Auth;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingByEmail = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );
        if (existingByEmail is not null)
        {
            throw new InvalidOperationException("Email is already in use.");
        }

        var effectiveUsername = string.IsNullOrWhiteSpace(request.Username)
            ? request.Email
            : request.Username;

        var existingByUsername = await _userRepository.GetByUsernameAsync(
            effectiveUsername,
            cancellationToken
        );
        if (existingByUsername is not null)
        {
            throw new InvalidOperationException("Username is already in use.");
        }

        var (hash, salt) = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(effectiveUsername, request.Email, hash, salt);
        await _userRepository.AddAsync(user, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Username, user.Email);

        return new AuthResult(user.Id, user.Username, user.Email, token);
    }
}
