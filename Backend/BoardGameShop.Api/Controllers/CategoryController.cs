using Microsoft.AspNetCore.Mvc;
using BoardGameShop.Api.Services;
using BoardGameShop.Api.Models.Category;
using System.Threading.Tasks;
using BoardGameShop.Api.Common;
using BoardGameShop.Api.Middleware;

namespace BoardGameShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [AuthorizeRole("Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> Create([FromBody] CreateCategoryDTO request)
        {
            try
            {
                var category = await _categoryService.CreateAsync(request);
                return CreatedAtAction(nameof(Create), new { id = category.Id },
                    ApiResponse<CategoryDTO>.Success(category, "Category created successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<CategoryDTO>.Error(400, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<CategoryDTO>.Error(409, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CategoryDTO>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<CategoryDTO>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}