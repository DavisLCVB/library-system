using LibrarySystem.Application.Categories;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class CategoryRepository(LibraryDbContext db) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(cancellationToken);
}
