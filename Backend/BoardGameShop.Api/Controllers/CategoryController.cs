using Microsoft.AspNetCore.Mvc;
using BoardGameShop.Api.Services;
using BoardGameShop.Api.Entities;
using System.Threading.Tasks;
using BoardGameShop.Api.Common;

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

        // GET: api/category
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Category>>>> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<Category>>.Success(categories, "Categories retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Category>>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // GET: api/category/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Category>>> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(ApiResponse<Category>.Success(category, "Category retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Category>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Category>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // POST: api/category
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Category>>> Create([FromBody] Category category)
        {
            try
            {
                var createdCategory = await _categoryService.CreateAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id },
                    ApiResponse<Category>.Success(createdCategory, "Category created successfully."));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<Category>.Error(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Category>.Error(400, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Category>.Error(409, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Category>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // PUT: api/category/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Category>>> Update(int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest(ApiResponse<Category>.Error(400, "Id mismatch between route and body."));

                var updatedCategory = await _categoryService.UpdateAsync(id, category);
                return Ok(ApiResponse<Category>.Success(updatedCategory, "Category updated successfully."));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<Category>.Error(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Category>.Error(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Category>.Error(404, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Category>.Error(409, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Category>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // DELETE: api/category/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Category>>> Delete(int id)
        {
            try
            {
                var category = await _categoryService.DeleteAsync(id);
                return Ok(ApiResponse<Category>.Success(category, "Category deleted successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Category>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Category>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}