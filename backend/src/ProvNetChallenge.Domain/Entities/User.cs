using System.ComponentModel.DataAnnotations;

namespace ProvNetChallenge.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool ActiveAccount { get; set; } = true; // 1= true, 0 = false

        [Required]
        public DateTime DateActivation { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DateDeactivation { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DateModification { get; set; } = DateTime.UtcNow;
    }
}