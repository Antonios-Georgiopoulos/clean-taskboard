using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public record GetSubtasksByTaskIdQuery(Guid TaskItemId) : IRequest<List<Subtask>>;
