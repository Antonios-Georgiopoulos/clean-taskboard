using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public class GetSubtasksByTaskIdQueryHandler
    : IRequestHandler<GetSubtasksByTaskIdQuery, List<Subtask>>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public GetSubtasksByTaskIdQueryHandler(
        ISubtaskRepository subtaskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _subtaskRepo = subtaskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<List<Subtask>> Handle(
        GetSubtasksByTaskIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadTask(
            request.TaskItemId,
            request.CurrentUserId,
            cancellationToken
        );

        return await _subtaskRepo.GetByTaskIdAsync(request.TaskItemId, cancellationToken);
    }
}
