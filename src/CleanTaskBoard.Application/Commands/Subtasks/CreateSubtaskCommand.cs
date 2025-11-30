using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record CreateSubtaskCommand(Guid CurrentUserId, Guid TaskItemId, string Title, int Order)
    : IRequest<Guid>;
