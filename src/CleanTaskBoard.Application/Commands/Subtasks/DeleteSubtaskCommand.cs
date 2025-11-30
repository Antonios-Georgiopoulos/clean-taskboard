using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record DeleteSubtaskCommand(Guid CurrentUserId, Guid SubtaskId) : IRequest<bool>;
