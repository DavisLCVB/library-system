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
}
