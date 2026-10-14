using LibrarySystem.Application.Categories;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class CategoryRepository(LibraryDbContext db) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(cancellationToken);

    public async Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) => await db.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public void Add(Category category) => db.Categories.Add(category);

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    ) => await db.Categories.AnyAsync(c => c.Name == name, cancellationToken);
}
