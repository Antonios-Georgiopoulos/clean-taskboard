using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Users;
using MediatR;

namespace CleanTaskBoard.Application.Auth.Queries;

public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _userRepository.GetByIdAsync(request.CurrentUserId, cancellationToken);
    }
}
