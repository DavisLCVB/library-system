using LibrarySystem.Application.Categories;
using LibrarySystem.Application.Common.Persistence;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Infrastructure.Persistence.Repositories;
using LibrarySystem.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool seedSampleData = false
    )
    {
        var connectionString =
            configuration.GetConnectionString("LibraryDb")
            ?? throw new InvalidOperationException(
                "Connection String 'LibraryDb' is not configured."
            );
        services.AddDbContext<LibraryDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            if (seedSampleData)
            {
                options.UseSeeding(LibraryDbSeeder.Seed).UseAsyncSeeding(LibraryDbSeeder.SeedAsync);
            }
        });
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LibraryDbContext>());
        return services;
    }
}
