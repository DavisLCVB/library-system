using LibrarySystem.Application.Categories;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString =
            configuration.GetConnectionString("LibraryDb")
            ?? throw new InvalidOperationException(
                "Connection String 'LibraryDb' is not configured."
            );
        services.AddDbContext<LibraryDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
