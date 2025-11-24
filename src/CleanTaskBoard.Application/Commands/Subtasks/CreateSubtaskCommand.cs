using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record CreateSubtaskCommand(Guid TaskItemId, string Title, int Order) : IRequest<Guid>;
