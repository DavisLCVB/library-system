using LibrarySystem.Application.Categories;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(
        int id,
        CancellationToken cancellationToken
    ) => Ok(await categoryService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var category = await categoryService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }
}
