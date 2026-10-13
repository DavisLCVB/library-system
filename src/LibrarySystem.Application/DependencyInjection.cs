using LibrarySystem.Application.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CategoryService>();
        return services;
    }
}
