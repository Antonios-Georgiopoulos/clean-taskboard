using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record UpdateTaskDetailsCommand(
    Guid Id,
    Guid OwnerUserId,
    string Title,
    string? Description,
    DateTime? DueDate,
    int Priority
) : IRequest<bool>;
