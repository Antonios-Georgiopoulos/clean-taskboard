using System.Security.Claims;
using System.Text;
using CleanTaskBoard.Api;
using CleanTaskBoard.Api.Middleware;
using CleanTaskBoard.Api.Requests.Auth;
using CleanTaskBoard.Api.Requests.Boards;
using CleanTaskBoard.Api.Requests.Column;
using CleanTaskBoard.Api.Requests.Subtask;
using CleanTaskBoard.Api.Requests.Task;
using CleanTaskBoard.Api.Responses;
using CleanTaskBoard.Api.Responses.Boards;
using CleanTaskBoard.Api.Responses.Column;
using CleanTaskBoard.Api.Responses.Subtask;
using CleanTaskBoard.Api.Responses.Task;
using CleanTaskBoard.Application;
using CleanTaskBoard.Application.Auth.Queries;
using CleanTaskBoard.Application.Commands.Auth;
using CleanTaskBoard.Application.Commands.Boards;
using CleanTaskBoard.Application.Commands.Columns;
using CleanTaskBoard.Application.Commands.Subtasks;
using CleanTaskBoard.Application.Commands.Tasks;
using CleanTaskBoard.Application.Queries.Boards;
using CleanTaskBoard.Application.Queries.Columns;
using CleanTaskBoard.Application.Queries.Subtasks;
using CleanTaskBoard.Application.Queries.Tasks;
using CleanTaskBoard.Domain.Entities.Boards;
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
app.UseMiddleware<ExceptionHandlingMiddleware>();

// BOARDS
app.MapPost(
        "/boards",
        async (CreateBoardRequest request, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var id = await mediator.Send(new CreateBoardCommand(userId, request.Name));

            return Results.Ok(new CreateBoardResponse(id));
        }
    )
    .RequireAuthorization();

app.MapGet(
        "/boards",
        async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var boards = await mediator.Send(new GetBoardsQuery(userId));
            return Results.Ok(boards);
        }
    )
    .RequireAuthorization();

app.MapGet(
        "/boards/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var board = await mediator.Send(new GetBoardByIdQuery(id, userId));

            return board is null ? Results.NotFound() : Results.Ok(board);
        }
    )
    .RequireAuthorization()
    .WithName("GetBoardById");

// BOARD MEMBERSHIPS
app.MapGet(
        "/boards/{boardId:guid}/members",
        async (Guid boardId, IMediator mediator, ClaimsPrincipal user) =>
        {
            var currentUserId = user.GetUserId();

            var members = await mediator.Send(new GetBoardMembersQuery(currentUserId, boardId));

            var response = members
                .Select(m => new BoardMemberResponse
                {
                    UserId = m.UserId,
                    Email = m.Email,
                    Role = m.Role,
                })
                .ToList();

            return Results.Ok(response);
        }
    )
    .RequireAuthorization()
    .WithName("GetBoardMembers");

app.MapPost(
        "/boards/{boardId:guid}/members",
        async (
            Guid boardId,
            AddBoardMemberRequest request,
            IMediator mediator,
            ClaimsPrincipal user
        ) =>
        {
            var currentUserId = user.GetUserId();

            if (!Enum.TryParse<BoardRole>(request.Role, ignoreCase: true, out var role))
            {
                return Results.BadRequest("Invalid role. Allowed: Owner, Member, Viewer");
            }

            var success = await mediator.Send(
                new AddBoardMemberCommand(currentUserId, boardId, request.MemberUserId, role)
            );

            return success ? Results.NoContent() : Results.BadRequest();
        }
    )
    .RequireAuthorization()
    .WithName("AddBoardMember");

app.MapPatch(
        "/boards/{boardId:guid}/members/{memberUserId:guid}",
        async (
            Guid boardId,
            Guid memberUserId,
            UpdateBoardMemberRoleRequest request,
            IMediator mediator,
            ClaimsPrincipal user
        ) =>
        {
            var currentUserId = user.GetUserId();

            if (!Enum.TryParse<BoardRole>(request.Role, ignoreCase: true, out var role))
            {
                return Results.BadRequest("Invalid role. Allowed: Owner, Member, Viewer");
            }

            var success = await mediator.Send(
                new UpdateBoardMemberRoleCommand(currentUserId, boardId, memberUserId, role)
            );

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("UpdateBoardMemberRole");

app.MapDelete(
        "/boards/{boardId:guid}/members/{memberUserId:guid}",
        async (Guid boardId, Guid memberUserId, IMediator mediator, ClaimsPrincipal user) =>
        {
            var currentUserId = user.GetUserId();

            var success = await mediator.Send(
                new RemoveBoardMemberCommand(currentUserId, boardId, memberUserId)
            );

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("RemoveBoardMember");

// COLUMNS

app.MapPost(
        "/boards/{boardId:guid}/columns",
        async (
            Guid boardId,
            CreateColumnRequest request,
            IMediator mediator,
            ClaimsPrincipal user
        ) =>
        {
            var userId = user.GetUserId();

            var command = new CreateColumnCommand(userId, boardId, request.Name, request.Order);

            var id = await mediator.Send(command);

            return Results.Ok(new CreateColumnResponse(id));
        }
    )
    .RequireAuthorization()
    .WithName("CreateColumn");

app.MapGet(
        "/boards/{boardId:guid}/columns",
        async (Guid boardId, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var columns = await mediator.Send(new GetColumnsByBoardIdQuery(boardId, userId));
            return Results.Ok(columns);
        }
    )
    .RequireAuthorization()
    .WithName("GetColumnsByBoardId");

app.MapGet(
        "/columns/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var column = await mediator.Send(new GetColumnByIdQuery(id, userId));

            return column is null ? Results.NotFound() : Results.Ok(column);
        }
    )
    .RequireAuthorization()
    .WithName("GetColumnById");

app.MapDelete(
        "/columns/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(new DeleteColumnCommand(id, userId));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("DeleteColumn");

// TASKS

app.MapPost(
        "/columns/{columnId:guid}/tasks",
        async (
            Guid columnId,
            CreateTaskRequest request,
            IMediator mediator,
            ClaimsPrincipal user
        ) =>
        {
            var userId = user.GetUserId();

            var command = new CreateTaskCommand(
                userId,
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
    .RequireAuthorization()
    .WithName("CreateTask");

app.MapGet(
        "/columns/{columnId:guid}/tasks",
        async (Guid columnId, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var tasks = await mediator.Send(new GetTasksByColumnIdQuery(columnId, userId));
            return Results.Ok(tasks);
        }
    )
    .RequireAuthorization()
    .WithName("GetTasksByColumnId");

app.MapGet(
        "/tasks/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var task = await mediator.Send(new GetTaskByIdQuery(id, userId));

            return task is null ? Results.NotFound() : Results.Ok(task);
        }
    )
    .RequireAuthorization()
    .WithName("GetTaskById");

app.MapPut(
        "/tasks/{id:guid}",
        async (Guid id, UpdateTaskRequest request, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var command = new UpdateTaskDetailsCommand(
                id,
                userId,
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority
            );

            var success = await mediator.Send(command);

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("UpdateTask");

app.MapDelete(
        "/tasks/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var success = await mediator.Send(new DeleteTaskCommand(id, userId));

            return success ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("DeleteTask");

app.MapPatch(
        "/tasks/{id:guid}/move",
        async (Guid id, MoveTaskRequest request, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(
                new MoveTaskCommand(id, userId, request.TargetColumnId, request.TargetPosition)
            );

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("MoveTask");

app.MapPatch(
        "/tasks/{id:guid}/complete",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(new CompleteTaskCommand(id, userId));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("CompleteTask");

app.MapPatch(
        "/tasks/{id:guid}/reorder",
        async (Guid id, ReorderTaskRequest request, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(
                new ReorderTaskCommand(id, userId, request.TargetPosition)
            );

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("ReorderTask");

// SUBTASKS

app.MapPost(
        "/tasks/{taskId:guid}/subtasks",
        async (
            Guid taskId,
            CreateSubtaskRequest request,
            IMediator mediator,
            ClaimsPrincipal user
        ) =>
        {
            var userId = user.GetUserId();

            var id = await mediator.Send(
                new CreateSubtaskCommand(userId, taskId, request.Title, request.Order)
            );

            return Results.Ok(new CreateSubtaskResponse(id));
        }
    )
    .RequireAuthorization()
    .WithName("CreateSubtask");

app.MapGet(
        "/tasks/{taskId:guid}/subtasks",
        async (Guid taskId, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var subtasks = await mediator.Send(new GetSubtasksByTaskIdQuery(taskId, userId));
            return Results.Ok(subtasks);
        }
    )
    .RequireAuthorization()
    .WithName("GetSubtasksByTaskId");

app.MapGet(
        "/subtasks/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var subtask = await mediator.Send(new GetSubtaskByIdQuery(id, userId));

            return subtask is null ? Results.NotFound() : Results.Ok(subtask);
        }
    )
    .RequireAuthorization()
    .WithName("GetSubtaskById");

app.MapPatch(
        "/subtasks/{id:guid}/complete",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(new CompleteSubtaskCommand(id, userId));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("CompleteSubtask");

app.MapPatch(
        "/subtasks/{id:guid}/reorder",
        async (Guid id, ReorderSubtaskRequest request, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(
                new ReorderSubtaskCommand(id, userId, request.TargetPosition)
            );

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("ReorderSubtask");

app.MapDelete(
        "/subtasks/{id:guid}",
        async (Guid id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();

            var result = await mediator.Send(new DeleteSubtaskCommand(id, userId));

            return result ? Results.NoContent() : Results.NotFound();
        }
    )
    .RequireAuthorization()
    .WithName("DeleteSubtask");

// AUTH
var authGroup = app.MapGroup("/auth");

authGroup
    .MapPost(
        "/register",
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

authGroup
    .MapPost(
        "/login",
        async (LoginRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new LoginCommand(request.Email, request.Password));

            return Results.Ok(
                new AuthResponse(result.UserId, result.Username, result.Email, result.Token)
            );
        }
    )
    .WithName("Login");

authGroup
    .MapGet(
        "/me",
        async (ClaimsPrincipal user, IMediator mediator) =>
        {
            var currentUserId = GetCurrentUserId(user);

            var currentUser = await mediator.Send(new GetCurrentUserQuery(currentUserId));

            if (currentUser is null)
            {
                return Results.NotFound();
            }

            var response = new MeResponse(
                currentUser.Id,
                currentUser.Email,
                currentUser.CreatedAtUtc
            );

            return Results.Ok(response);
        }
    )
    .RequireAuthorization()
    .WithName("GetCurrentUser");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

static Guid GetCurrentUserId(ClaimsPrincipal user)
{
    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");

    if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id))
    {
        throw new InvalidOperationException("Invalid or missing user id in JWT token.");
    }

    return id;
}

app.Run();
