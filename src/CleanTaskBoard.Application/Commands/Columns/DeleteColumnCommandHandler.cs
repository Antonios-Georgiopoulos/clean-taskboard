using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IColumnRepository _columnRepository;

    public DeleteColumnCommandHandler(IColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.Id, cancellationToken);

        if (column is null)
            return false;

        await _columnRepository.DeleteAsync(column, cancellationToken);

        return true;
    }
}
