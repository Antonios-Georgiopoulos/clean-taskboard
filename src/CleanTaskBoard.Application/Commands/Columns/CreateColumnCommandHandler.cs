using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities.Column;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Guid>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardAccessService _boardAccessService;

    public CreateColumnCommandHandler(
        IColumnRepository columnRepository,
        IBoardAccessService boardAccessService
    )
    {
        _columnRepository = columnRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<Guid> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        // Here you need rights on the BOARD (only the Owner can create columns).
        await _boardAccessService.EnsureCanEditBoard(
            request.BoardId,
            request.CurrentUserId,
            cancellationToken
        );

        var column = new Column
        {
            Id = Guid.NewGuid(),
            BoardId = request.BoardId,
            Name = request.Name,
            Order = request.Order,
        };

        return await _columnRepository.AddAsync(column, cancellationToken);
    }
}
