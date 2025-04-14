using Microsoft.AspNetCore.Mvc;
using BoardGameShop.Api.Services;
using BoardGameShop.Api.Models.User;
using System.Threading.Tasks;
using BoardGameShop.Api.Common;
using BoardGameShop.Api.Middleware; // Thêm namespace để sử dụng AllowAnonymous

namespace BoardGameShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous] // Thêm attribute này
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] CreateUserDTO request)
        {
            try
            {
                var user = await _userService.RegisterAsync(request);
                return CreatedAtAction(nameof(GetUser), new { username = user.Username },
                    ApiResponse<UserDTO>.Success(user, "User registered successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<UserDTO>.Error(400, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<UserDTO>.Error(409, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserDTO>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous] // Thêm attribute này
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _userService.LoginAsync(request.Username, request.Password);
                return Ok(ApiResponse<string>.Success(token, "Login successful."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(400, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<string>.Error(401, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet("user/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> GetUser(string username)
        {
            try
            {
                var user = await _userService.GetByUsernameAsync(username);
                return Ok(ApiResponse<UserDTO>.Success(user, "User retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<UserDTO>.Error(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserDTO>.Error(500, $"Internal server error: {ex.Message}"));
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}