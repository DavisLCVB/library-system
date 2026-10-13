using LibrarySystem.Application.Categories;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategoryService categoryService) : ControllerBase
{
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }
}
