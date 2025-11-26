using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public class GetBoardsQueryHandler : IRequestHandler<GetBoardsQuery, List<Board>>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardsQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<List<Board>> Handle(
        GetBoardsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _boardRepository.GetByOwnerAsync(request.OwnerUserId, cancellationToken);
    }
}
