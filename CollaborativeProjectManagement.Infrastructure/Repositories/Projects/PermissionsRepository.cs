using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Projects
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly AppDbContext _dbContext;

        public PermissionsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PermissionEntity>?> GetAllPermissionsAsync()
        {
            return await _dbContext.Permissions.ToListAsync();   
        }
    }
}