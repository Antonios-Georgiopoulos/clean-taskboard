using CleanTaskBoard.Domain.Entities.Users;
using MediatR;

namespace CleanTaskBoard.Application.Auth.Queries;

public sealed record GetCurrentUserQuery(Guid CurrentUserId) : IRequest<User?>;
