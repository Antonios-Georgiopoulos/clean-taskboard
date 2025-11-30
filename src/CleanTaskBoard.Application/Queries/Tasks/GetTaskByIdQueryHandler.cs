using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItem?>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public GetTaskByIdQueryHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<TaskItem?> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadTask(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        return await _taskRepo.GetByIdAsync(request.Id, cancellationToken);
    }
}
