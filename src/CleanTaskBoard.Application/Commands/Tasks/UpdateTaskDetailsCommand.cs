using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record UpdateTaskDetailsCommand(
    Guid CurrentUserId,
    Guid Id,
    string Title,
    string? Description,
    DateTime? DueDate,
    int Priority
) : IRequest<bool>;
