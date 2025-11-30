using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public record GetTasksByColumnIdQuery(Guid CurrentUserId, Guid ColumnId) : IRequest<List<TaskItem>>;
