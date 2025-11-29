using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public class GetTasksByColumnIdQueryHandler
    : IRequestHandler<GetTasksByColumnIdQuery, List<TaskItem>>
{
    private readonly ITaskItemRepository _taskRepo;

    public GetTasksByColumnIdQueryHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<List<TaskItem>> Handle(
        GetTasksByColumnIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _taskRepo.GetByColumnIdAsync(
            request.ColumnId,
            request.OwnerUserId,
            cancellationToken
        );
    }
}
