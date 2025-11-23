using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Tasks;

public record GetTaskByIdQuery(Guid Id) : IRequest<TaskItem?>;
