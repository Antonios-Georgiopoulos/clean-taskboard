using CleanTaskBoard.Api.Requests;
using CleanTaskBoard.Api.Responses;
using CleanTaskBoard.Application;
using CleanTaskBoard.Application.Commands.Boards;
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
