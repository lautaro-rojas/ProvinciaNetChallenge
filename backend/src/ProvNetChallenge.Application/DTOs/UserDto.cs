namespace ProvNetChallenge.Application.DTOs
{
    public class UserDto
    {
        // Este objeto se usa SOLO para enviar datos desde el Servicio hacia afuera.
        // Para que "afuera" no vea el Password ni otros datos sensibles.
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime DateActivation { get; set; }
        public DateTime? DateModification { get; set; }
        public DateTime? DateDeactivation { get; set; }   
    }
}