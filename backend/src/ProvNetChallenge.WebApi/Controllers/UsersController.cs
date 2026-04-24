using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces;

namespace ProvNetChallenge.WebApi.Controllers
{   
    [Authorize] // Protect all endpoints in this controller with JWT authentication
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserDto>>> UserGetAll()
        {
            var usersDto = await _userService.GetAllAsync();

            if (usersDto == null || usersDto.Count == 0)
            {
                return NotFound(new { message = "No users found." });
            }

            return Ok(usersDto);
        }

        // GET: api/users/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UserGetByID(int id)
        {
            var userDto = await _userService.GetByIdAsync(id);
            
            if (userDto == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return Ok(userDto);
        }

        // POST: api/users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> UserCreate([FromBody] UserCreationDto userCreationDto)
        {
            // Si el email está repetido, nuestro Service lanza BadRequestException y lo ataja el Middleware.

            int newUserId = await _userService.AddAsync(userCreationDto);

            return CreatedAtAction(nameof(UserGetByID), new { id = newUserId }, new { id = newUserId, message = "User created successfully." }); // Code 201: User created
        }

        // PUT: api/users/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserUpdate([FromRoute] int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var success = await _userService.UpdateAsync(id, userUpdateDto);

            if (!success)
            {
                return NotFound(new { message = $"User with ID {id} not found." }); //Code 404
            }

            return NoContent(); // Code 204: The update was successful but there is no content to return
        }

        // DELETE: api/users/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserDelete(int id)
        {
            var success = await _userService.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"User with ID {id} not found." }); //Code 404
            }

            return NoContent(); // Code 204: The update was successful but there is no content to return
        }

        // DELETE LÓGICO: api/users/5/logic
        [HttpDelete("{id:int}/logic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserDeleteLogic(int id)
        {
            var success = await _userService.DeleteLogicAsync(id);

            if (!success)
            {
                return NotFound(new { message = $"User with ID {id} not found." }); //Code 404
            }

            return NoContent(); // Code 204: The update was successful but there is no content to return
        }
    }
}