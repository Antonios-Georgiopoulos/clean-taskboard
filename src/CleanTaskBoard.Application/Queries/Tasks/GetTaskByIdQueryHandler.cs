using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItem?>
{
    private readonly ITaskItemRepository _taskRepo;

    public GetTaskByIdQueryHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<TaskItem?> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _taskRepo.GetByIdAsync(request.Id, request.OwnerUserId, cancellationToken);
    }
}
