using System.ComponentModel.DataAnnotations;

namespace ProvNetChallenge.Application.DTOs
{
    public class UserUpdateDto
    {
        // Este objeto se usa SOLO para recibir datos de "afuera" cuando queremos ACTUALIZAR un usuario.

        [Required(ErrorMessage = "Id is required")]    
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }
}