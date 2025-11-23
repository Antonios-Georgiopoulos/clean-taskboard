using CleanTaskBoard.Api.Requests;
using CleanTaskBoard.Api.Responses;
using CleanTaskBoard.Application;
using CleanTaskBoard.Application.Commands.Boards;
using CleanTaskBoard.Application.Commands.Columns;
using CleanTaskBoard.Application.Commands.Tasks;
using CleanTaskBoard.Application.Queries.Boards;
using CleanTaskBoard.Application.Queries.Columns;
using CleanTaskBoard.Application.Queries.Tasks;
using CleanTaskBoard.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapPost(
    "/boards",
    async (CreateBoardRequest request, IMediator mediator) =>
    {
        var id = await mediator.Send(new CreateBoardCommand(request.Name));
        return Results.Ok(new CreateBoardResponse(id));
    }
);

app.MapPost(
        "/boards/{boardId:guid}/columns",
        async (Guid boardId, CreateColumnRequest request, IMediator mediator) =>
        {
            var command = new CreateColumnCommand(boardId, request.Name, request.Order);
            var id = await mediator.Send(command);

            return Results.Ok(new CreateColumnResponse(id));
        }
    )
    .WithName("CreateColumn");

app.MapGet(
    "/boards",
    async (IMediator mediator) =>
    {
        var boards = await mediator.Send(new GetBoardsQuery());
        return Results.Ok(boards);
    }
);

app.MapGet(
        "/boards/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var board = await mediator.Send(new GetBoardByIdQuery(id));

            return board is null ? Results.NotFound() : Results.Ok(board);
        }
    )
    .WithName("GetBoardById");

app.MapGet(
        "/boards/{boardId:guid}/columns",
        async (Guid boardId, IMediator mediator) =>
        {
            var columns = await mediator.Send(new GetColumnsByBoardIdQuery(boardId));
            return Results.Ok(columns);
        }
    )
    .WithName("GetColumnsByBoardId");

app.MapGet(
        "/columns/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var column = await mediator.Send(new GetColumnByIdQuery(id));

            return column is null ? Results.NotFound() : Results.Ok(column);
        }
    )
    .WithName("GetColumnById");

app.MapDelete(
        "/columns/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteColumnCommand(id));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("DeleteColumn");

// TASKS

app.MapPost(
        "/columns/{columnId:guid}/tasks",
        async (Guid columnId, CreateTaskRequest request, IMediator mediator) =>
        {
            var command = new CreateTaskCommand(
                columnId,
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority
            );

            var id = await mediator.Send(command);

            return Results.Ok(new CreateTaskResponse(id));
        }
    )
    .WithName("CreateTask");

app.MapGet(
        "/columns/{columnId:guid}/tasks",
        async (Guid columnId, IMediator mediator) =>
        {
            var tasks = await mediator.Send(new GetTasksByColumnIdQuery(columnId));
            return Results.Ok(tasks);
        }
    )
    .WithName("GetTasksByColumnId");

app.MapGet(
        "/tasks/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var task = await mediator.Send(new GetTaskByIdQuery(id));

            return task is null ? Results.NotFound() : Results.Ok(task);
        }
    )
    .WithName("GetTaskById");

app.MapPut(
        "/tasks/{id:guid}",
        async (Guid id, UpdateTaskRequest request, IMediator mediator) =>
        {
            var command = new UpdateTaskDetailsCommand(
                id,
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority
            );

            var success = await mediator.Send(command);

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("UpdateTask");

app.MapDelete(
        "/tasks/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var success = await mediator.Send(new DeleteTaskCommand(id));

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("DeleteTask");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
