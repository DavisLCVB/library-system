using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Categories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Category category);
    Task<bool> ExistsByNameAsync(
        string name,
        int? excludingId = null,
        CancellationToken cancellationToken = default
    );
    Task<bool> HasBookAsync(int id, CancellationToken cancellationToken = default);
    void Remove(Category category);
}
