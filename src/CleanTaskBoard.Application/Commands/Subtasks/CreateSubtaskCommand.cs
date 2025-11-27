using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record CreateSubtaskCommand(Guid OwnerUserId, Guid TaskItemId, string Title, int Order)
    : IRequest<Guid>;
