using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Subtasks;

public record GetSubtaskByIdQuery(Guid Id, Guid OwnerUserId) : IRequest<Subtask?>;
