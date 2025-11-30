using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public class GetColumnsByBoardIdQueryHandler
    : IRequestHandler<GetColumnsByBoardIdQuery, List<Column>>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardAccessService _boardAccessService;

    public GetColumnsByBoardIdQueryHandler(
        IColumnRepository columnRepository,
        IBoardAccessService boardAccessService
    )
    {
        _columnRepository = columnRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<List<Column>> Handle(
        GetColumnsByBoardIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadBoard(
            request.BoardId,
            request.CurrentUserId,
            cancellationToken
        );

        return await _columnRepository.GetByBoardIdAsync(request.BoardId, cancellationToken);
    }
}
