using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record CompleteSubtaskCommand(Guid CurrentUserId, Guid SubtaskId) : IRequest<bool>;
