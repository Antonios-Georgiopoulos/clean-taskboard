using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public class GetColumnsByBoardIdQueryHandler
    : IRequestHandler<GetColumnsByBoardIdQuery, List<Column>>
{
    private readonly IColumnRepository _columnRepository;

    public GetColumnsByBoardIdQueryHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<List<Column>> Handle(
        GetColumnsByBoardIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _columnRepository.GetByBoardIdAsync(
            request.BoardId,
            request.OwnerUserId,
            cancellationToken
        );
    }
}
