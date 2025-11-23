using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public record DeleteColumnCommand(Guid Id) : IRequest<bool>;
