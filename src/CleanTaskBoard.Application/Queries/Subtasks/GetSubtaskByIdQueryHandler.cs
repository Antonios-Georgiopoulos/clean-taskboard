using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public class GetSubtaskByIdQueryHandler : IRequestHandler<GetSubtaskByIdQuery, Subtask?>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public GetSubtaskByIdQueryHandler(
        ISubtaskRepository subtaskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _subtaskRepo = subtaskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<Subtask?> Handle(
        GetSubtaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanReadSubtask(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        return await _subtaskRepo.GetByIdAsync(request.Id, cancellationToken);
    }
}
