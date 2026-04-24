using Microsoft.AspNetCore.Mvc;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.DTOs;

namespace ProvNetChallenge.WebApi.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            
            if (result == null) 
                return Unauthorized(new { message = "Incorrect credentials" });

            return Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] UserCreationDto dto)
        {
            var newUserId = await _userService.AddAsync(dto);
            
            // Retornamos un 201 Created (Buenas prácticas REST)
            return Created("", new { 
                message = "User registered successfully", 
                userId = newUserId 
            });
        }
    }
}