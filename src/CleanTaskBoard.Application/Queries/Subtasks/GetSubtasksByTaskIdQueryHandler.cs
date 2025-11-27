using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public class GetSubtasksByTaskIdQueryHandler
    : IRequestHandler<GetSubtasksByTaskIdQuery, List<Subtask>>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public GetSubtasksByTaskIdQueryHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<List<Subtask>> Handle(
        GetSubtasksByTaskIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _subtaskRepo.GetByTaskIdAsync(
            request.TaskItemId,
            request.OwnerUserId,
            cancellationToken
        );
    }
}
