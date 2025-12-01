using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities.Column;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public class GetColumnByIdQueryHandler : IRequestHandler<GetColumnByIdQuery, Column?>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardAccessService _boardAccessService;

    public GetColumnByIdQueryHandler(
        IColumnRepository columnRepository,
        IBoardAccessService boardAccessService
    )
    {
        _columnRepository = columnRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<Column?> Handle(
        GetColumnByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadColumn(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        return await _columnRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
