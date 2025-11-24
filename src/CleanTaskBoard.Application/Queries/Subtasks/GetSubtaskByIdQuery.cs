using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public record GetSubtaskByIdQuery(Guid Id) : IRequest<Subtask?>;
