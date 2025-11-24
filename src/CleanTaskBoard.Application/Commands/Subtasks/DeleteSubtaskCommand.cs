using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record DeleteSubtaskCommand(Guid SubtaskId) : IRequest<bool>;
