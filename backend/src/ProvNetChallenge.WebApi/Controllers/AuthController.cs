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
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            UserDto? user = await _authService.LoginAsync(loginDto);

            if (user != null) 
            {
                var tokenString = _jwtService.GenerateJwtToken(user.Id.ToString(), user.Email, user.UserName);

                return Ok(new { Token = tokenString }); // Code 200: OK
            }

            return Unauthorized(new { Mensaje = "Incorrect credentials" }); // Code 401: Unauthorized
        }
    }
}