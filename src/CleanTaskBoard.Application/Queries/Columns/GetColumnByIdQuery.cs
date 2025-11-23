using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public record GetColumnByIdQuery(Guid Id) : IRequest<Column?>;
