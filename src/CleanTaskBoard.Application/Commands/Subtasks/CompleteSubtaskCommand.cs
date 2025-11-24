using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record CompleteSubtaskCommand(Guid SubtaskId) : IRequest<bool>;
