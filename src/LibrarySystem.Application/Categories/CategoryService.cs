using LibrarySystem.Application.Common.Exceptions;
using LibrarySystem.Application.Common.Persistence;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Categories;

public class CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork)
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

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var category = new Category { Name = request.Name.Trim() };
        if (await repository.ExistsByNameAsync(
                category.Name,
                cancellationToken: cancellationToken
            )
        )
        {
            throw new ConflictException($"Category with name '{category.Name}' already exists.");
        }
        repository.Add(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(category);
    }

    private static CategoryDto ToDto(Category category) => new(category.Id, category.Name);

    public async Task UpdateAsync(
        int id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var category =
            await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), id);

        var newName = request.Name.Trim();
        if (
            category.Name != newName
            && await repository.ExistsByNameAsync(newName, id, cancellationToken)
        )
        {
            throw new ConflictException($"Category with name '{newName}' already exists.");
        }

        category.Name = newName;
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category =
            await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), id);

        if (await repository.HasBookAsync(id, cancellationToken))
        {
            throw new ConflictException(
                $"Cannot delete category '{category.Name}' because it has associated books."
            );
        }

        repository.Remove(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
