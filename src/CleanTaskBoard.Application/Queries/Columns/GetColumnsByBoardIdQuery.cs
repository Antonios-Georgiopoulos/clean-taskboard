using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Columns;

public record GetColumnsByBoardIdQuery(Guid BoardId, Guid OwnerUserId) : IRequest<List<Column>>;
