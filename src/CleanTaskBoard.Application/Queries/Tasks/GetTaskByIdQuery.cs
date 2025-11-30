using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public record GetTaskByIdQuery(Guid CurrentUserId, Guid Id) : IRequest<TaskItem?>;
