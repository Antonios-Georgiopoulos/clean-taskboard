namespace CleanTaskBoard.Application.Commands.Boards;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Guid>
{
    private readonly IBoardRepository _boardRepository;

    public CreateBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        return await _boardRepository.AddAsync(board, cancellationToken);
    }
}
