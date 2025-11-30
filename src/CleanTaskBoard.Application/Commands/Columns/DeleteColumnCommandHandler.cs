using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardAccessService _boardAccessService;

    public DeleteColumnCommandHandler(
        IColumnRepository columnRepository,
        IBoardAccessService boardAccessService
    )
    {
        _columnRepository = columnRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanEditColumn(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        var column = await _columnRepository.GetByIdAsync(request.Id, cancellationToken);
        if (column is null)
            return false;

        await _columnRepository.DeleteAsync(column, cancellationToken);
        return true;
    }
}
