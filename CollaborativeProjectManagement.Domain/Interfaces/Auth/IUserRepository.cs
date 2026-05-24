using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Domain.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User data);
        Task<bool> CheckIfEmailExistsAsync(string email);
        Task<User?> GetUserById(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<UserRole> GetUserRoleId(Guid userId);
        Task<List<User>> GetUsersListByEmail(List<string> userEmails);
    }
}
