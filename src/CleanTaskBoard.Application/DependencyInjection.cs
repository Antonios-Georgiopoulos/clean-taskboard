using System.Reflection;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTaskBoard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddScoped<IBoardAccessService, BoardAccessService>();

        return services;
    }
}
