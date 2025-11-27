using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Guid>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;

    public CreateColumnCommandHandler(
        IColumnRepository columnRepository,
        IBoardRepository boardRepository
    )
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }

    public async Task<Guid> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        // Εξασφάλιση ότι το board ανήκει στον χρήστη
        _ =
            await _boardRepository.GetByIdAsync(
                request.BoardId,
                request.OwnerUserId,
                cancellationToken
            ) ?? throw new InvalidOperationException("Board not found or access denied.");

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
