using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Board?>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardAccessService _boardAccessService;

    public GetBoardByIdQueryHandler(
        IBoardRepository boardRepository,
        IBoardAccessService boardAccessService
    )
    {
        _boardRepository = boardRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<Board?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanReadBoard(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        return await _boardRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
