using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Application.DTOs.Auth
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        
        public static UserDTO FromEntity(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }
}
