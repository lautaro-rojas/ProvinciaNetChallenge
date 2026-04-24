namespace ProvNetChallenge.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true; // 1= true, 0 = false

        public DateTime DateActivation { get; set; } = DateTime.UtcNow;

        public DateTime? DateDeactivation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}