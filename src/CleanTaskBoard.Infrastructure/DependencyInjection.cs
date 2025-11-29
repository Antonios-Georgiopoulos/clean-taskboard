using CleanTaskBoard.Application.Auth;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Infrastructure.Persistence;
using CleanTaskBoard.Infrastructure.Repositories;
using CleanTaskBoard.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTaskBoard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<ISubtaskRepository, SubtaskRepository>();
            services.AddScoped<IBoardMembershipRepository, BoardMembershipRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
