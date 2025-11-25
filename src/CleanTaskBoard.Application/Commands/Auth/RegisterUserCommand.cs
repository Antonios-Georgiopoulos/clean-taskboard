using CleanTaskBoard.Application.Auth;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Auth;

public record RegisterUserCommand(string Username, string Email, string Password)
    : IRequest<AuthResult>;
