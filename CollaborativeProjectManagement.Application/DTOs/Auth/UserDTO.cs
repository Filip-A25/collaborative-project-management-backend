using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Application.DTOs.Auth
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required UserRole Role { get; set; }
    }
}
