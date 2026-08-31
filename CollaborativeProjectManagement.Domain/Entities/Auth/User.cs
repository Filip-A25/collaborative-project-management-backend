using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Domain.Entities.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Username { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User(string fistName, string lastName, string username, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            FirstName = fistName;
            LastName = lastName;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private User() { }
    }
}
