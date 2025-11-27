using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public record GetTasksByColumnIdQuery(Guid ColumnId, Guid OwnerUserId) : IRequest<List<TaskItem>>;
