using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Auth
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User data)
        {
            _dbContext.Users.Add(data);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public async Task<bool> CheckIfEmailExistsAsync(string email)
        {
            User? user = await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id ==userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<UserRole> GetUserRoleId(Guid userId)
        {
            return await _dbContext.Users.Where(u => u.Id == userId).Select(u => u.Role).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersListByEmail(List<string> userEmails)
        {
            return await _dbContext.Users.Where(user => userEmails.Contains(user.Email)).ToListAsync();
        }
    }
}
