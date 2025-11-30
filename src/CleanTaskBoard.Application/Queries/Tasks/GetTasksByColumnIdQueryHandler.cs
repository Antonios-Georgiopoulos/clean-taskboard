using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public class GetTasksByColumnIdQueryHandler
    : IRequestHandler<GetTasksByColumnIdQuery, List<TaskItem>>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public GetTasksByColumnIdQueryHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<List<TaskItem>> Handle(
        GetTasksByColumnIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadColumn(
            request.ColumnId,
            request.CurrentUserId,
            cancellationToken
        );

        return await _taskRepo.GetByColumnIdAsync(request.ColumnId, cancellationToken);
    }
}
