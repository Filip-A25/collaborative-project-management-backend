using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories
{
    public class ProjectInvitesRepository : IProjectInvitesRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectInvitesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateProjectInviteAsync(ProjectInvite projectInvites)
        {
            _dbContext.ProjectInvites.Add(projectInvites);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectInviteAsync(Guid projectId, int inviteId)
        {
            await _dbContext.ProjectInvites.Where(invite => invite.ProjectId == projectId).Where(invite => invite.Id == inviteId).ExecuteDeleteAsync();
        }

        public async Task<ProjectInvite?> GetProjectInviteAsync(Guid projectId, int inviteId)
        {
            return await _dbContext.ProjectInvites.Where(invite => invite.ProjectId == projectId).FirstOrDefaultAsync(invite => invite.Id == inviteId);
        }

        public async Task UpdateProjectInviteToAcceptedAsync(ProjectInvite invite)
        {
            invite.IsAccepted = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProjectInvite>?> GetAllUserInvitesAsync(Guid userId)
        {
            return await _dbContext.ProjectInvites.Where(invite => invite.InvitedUserId == userId).Where(invite => invite.ExpiresAt > DateTime.UtcNow).Where(invite => !invite.IsAccepted).ToListAsync();
        }

        public async Task<List<ProjectInvite>?> GetAllProjectsInvitesAsync(Guid projectId)
        {
            return await _dbContext.ProjectInvites.Where(invite => invite.ProjectId == projectId).Where(invite => invite.ExpiresAt > DateTime.UtcNow).ToListAsync();
        }
    }
}
