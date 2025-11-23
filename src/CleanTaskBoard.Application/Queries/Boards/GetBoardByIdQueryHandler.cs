using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Board?>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Board?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        return await _boardRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
