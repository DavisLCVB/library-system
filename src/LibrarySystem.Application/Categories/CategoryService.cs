using LibrarySystem.Application.Common.Exceptions;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Categories;

public class CategoryService(ICategoryRepository repository)
{
    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var categories = await repository.GetAllAsync(cancellationToken);
        return categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
    }

    public async Task<CategoryDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var category =
            await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), id);

        return ToDto(category);
    }

    private static CategoryDto ToDto(Category category) => new(category.Id, category.Name);
}
