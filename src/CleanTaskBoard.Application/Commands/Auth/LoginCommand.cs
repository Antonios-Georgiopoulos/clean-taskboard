using CleanTaskBoard.Application.Auth;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Auth;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
