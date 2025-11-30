using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities;
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
        // Μόνο Owner μπορεί να πειράξει columns
        await _boardAccessService.EnsureCanEditColumn(
            columnId: request.BoardId, // Εδώ καλύτερα να κάνεις EnsureCanEditBoard ή δικό σου helper
            userId: request.CurrentUserId,
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
