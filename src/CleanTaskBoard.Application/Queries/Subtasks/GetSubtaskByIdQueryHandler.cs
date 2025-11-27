using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public class GetSubtaskByIdQueryHandler : IRequestHandler<GetSubtaskByIdQuery, Subtask?>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public GetSubtaskByIdQueryHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<Subtask?> Handle(
        GetSubtaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _subtaskRepo.GetByIdAsync(request.Id, request.OwnerUserId, cancellationToken);
    }
}
