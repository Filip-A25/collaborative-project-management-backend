using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Domain.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User data);
        Task<bool> CheckIfEmailExistsAsync(string email);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<User>> GetUsersListByEmailAsync(List<string> userEmails);
        Task UpdateUserAsync();
        Task<bool> CheckForExistingUsernameAsync(Guid userId, string username);
        Task<bool> CheckForExistingEmailAsync(Guid userId, string email);
    }
}
