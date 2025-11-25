using System.Text;
using CleanTaskBoard.Api.Requests;
using CleanTaskBoard.Api.Requests.Auth;
using CleanTaskBoard.Api.Responses;
using CleanTaskBoard.Application;
using CleanTaskBoard.Application.Commands.Auth;
using CleanTaskBoard.Application.Commands.Boards;
using CleanTaskBoard.Application.Commands.Columns;
using CleanTaskBoard.Application.Commands.Subtasks;
using CleanTaskBoard.Application.Commands.Tasks;
using CleanTaskBoard.Application.Queries.Boards;
using CleanTaskBoard.Application.Queries.Columns;
using CleanTaskBoard.Application.Queries.Subtasks;
using CleanTaskBoard.Application.Queries.Tasks;
using CleanTaskBoard.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// OpenAPI / Swagger
builder.Services.AddOpenApi();

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection.GetValue<string>("Secret");

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection.GetValue<string>("Issuer"),
            ValidAudience = jwtSection.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!)),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

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

app.MapPatch(
        "/tasks/{id:guid}/move",
        async (Guid id, MoveTaskRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(
                new MoveTaskCommand(id, request.TargetColumnId, request.TargetPosition)
            );

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("MoveTask");

app.MapPatch(
        "/tasks/{id:guid}/complete",
        async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new CompleteTaskCommand(id));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("CompleteTask");

app.MapPatch(
        "/tasks/{id:guid}/reorder",
        async (Guid id, ReorderTaskRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ReorderTaskCommand(id, request.TargetPosition));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("ReorderTask");

// SUBTASKS

app.MapPost(
        "/tasks/{taskId:guid}/subtasks",
        async (Guid taskId, CreateSubtaskRequest request, IMediator mediator) =>
        {
            var id = await mediator.Send(
                new CreateSubtaskCommand(taskId, request.Title, request.Order)
            );

            return Results.Ok(new CreateSubtaskResponse(id));
        }
    )
    .WithName("CreateSubtask");

app.MapGet(
        "/tasks/{taskId:guid}/subtasks",
        async (Guid taskId, IMediator mediator) =>
        {
            var subtasks = await mediator.Send(new GetSubtasksByTaskIdQuery(taskId));
            return Results.Ok(subtasks);
        }
    )
    .WithName("GetSubtasksByTaskId");

app.MapGet(
        "/subtasks/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var subtask = await mediator.Send(new GetSubtaskByIdQuery(id));

            return subtask is null ? Results.NotFound() : Results.Ok(subtask);
        }
    )
    .WithName("GetSubtaskById");

app.MapPatch(
        "/subtasks/{id:guid}/complete",
        async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new CompleteSubtaskCommand(id));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("CompleteSubtask");

app.MapPatch(
        "/subtasks/{id:guid}/reorder",
        async (Guid id, ReorderSubtaskRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ReorderSubtaskCommand(id, request.TargetPosition));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("ReorderSubtask");

app.MapDelete(
        "/subtasks/{id:guid}",
        async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteSubtaskCommand(id));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .WithName("DeleteSubtask");

// AUTH

app.MapPost(
        "/auth/register",
        async (RegisterRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(
                new RegisterUserCommand(request.Username, request.Email, request.Password)
            );

            return Results.Ok(
                new AuthResponse(result.UserId, result.Username, result.Email, result.Token)
            );
        }
    )
    .WithName("Register");

app.MapPost(
        "/auth/login",
        async (LoginRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new LoginCommand(request.Email, request.Password));

            return Results.Ok(
                new AuthResponse(result.UserId, result.Username, result.Email, result.Token)
            );
        }
    )
    .WithName("Login");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
