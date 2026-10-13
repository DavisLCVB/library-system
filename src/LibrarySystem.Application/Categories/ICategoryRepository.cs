using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Categories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);
}
