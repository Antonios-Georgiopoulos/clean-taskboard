using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public record GetColumnsByBoardIdQuery(Guid CurrentUserId, Guid BoardId) : IRequest<List<Column>>;
