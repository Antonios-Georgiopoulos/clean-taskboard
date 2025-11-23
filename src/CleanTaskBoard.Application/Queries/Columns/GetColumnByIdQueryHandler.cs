using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public class GetColumnByIdQueryHandler : IRequestHandler<GetColumnByIdQuery, Column?>
{
    private readonly IColumnRepository _columnRepository;

    public GetColumnByIdQueryHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<Column?> Handle(
        GetColumnByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _columnRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
