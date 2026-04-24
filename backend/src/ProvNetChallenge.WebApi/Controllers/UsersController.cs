using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces;

namespace ProvNetChallenge.WebApi.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect all endpoints in this controller with JWT authentication
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int newUserId = await _userService.AddAsync(userCreationDto);

                var userFromDb = await _userService.GetByIdAsync(newUserId);

                return CreatedAtAction(nameof(UserGetByID), new { id = newUserId }, userFromDb); // Code 201: User created

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        // PUT: api/users/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserUpdate([FromRoute] int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (id != userUpdateDto.Id)
            {
                return BadRequest(new { message = "The ID in the route does not match the ID in the request body." }); //Code 400
            }

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

        // DELETE LÓGICO: api/users/5/logical
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