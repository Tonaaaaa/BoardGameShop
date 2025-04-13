using Microsoft.AspNetCore.Mvc;
using BoardGameShop.Api.Services;
using BoardGameShop.Api.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BoardGameShop.Api.Common;

namespace BoardGameShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<Product>>.Success(products, "Products retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Product>>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // GET: api/product/{slug}
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> GetBySlug(string slug)
        {
            try
            {
                var product = await _productService.GetBySlugAsync(slug);
                return Ok(ApiResponse<Product>.Success(product, "Product retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Product>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // POST: api/product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> Create([FromForm] Product product, IFormFile image)
        {
            try
            {
                var createdProduct = await _productService.CreateAsync(product, image);
                return CreatedAtAction(nameof(GetBySlug), new { slug = createdProduct.Slug },
                    ApiResponse<Product>.Success(createdProduct, "Product created successfully."));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<Product>.Error(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Product>.Error(400, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Product>.Error(409, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // PUT: api/product/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> Update(int id, [FromForm] Product product, IFormFile image)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest(ApiResponse<Product>.Error(400, "Id mismatch between route and body."));

                var updatedProduct = await _productService.UpdateAsync(id, product, image);
                return Ok(ApiResponse<Product>.Success(updatedProduct, "Product updated successfully."));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<Product>.Error(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<Product>.Error(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Product>.Error(404, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<Product>.Error(409, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Product>>> Delete(int id)
        {
            try
            {
                var product = await _productService.DeleteAsync(id);
                return Ok(ApiResponse<Product>.Success(product, "Product deleted successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<Product>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}