using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public record GetTaskByIdQuery(Guid Id, Guid OwnerUserId) : IRequest<TaskItem?>;
