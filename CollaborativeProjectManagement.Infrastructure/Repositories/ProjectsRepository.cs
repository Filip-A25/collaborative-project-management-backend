using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<ProjectRole> CreateProjectRoleAsync(ProjectRole projectRole)
        {
            _dbContext.ProjectRoles.Add(projectRole);
            await _dbContext.SaveChangesAsync();

            return projectRole;
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            await _dbContext.Projects.Where(project => project.Id == projectId).ExecuteDeleteAsync();
        }

        public async Task AddMemberToProjectAsync(ProjectMember member)
        {
            _dbContext.ProjectMembers.Add(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project?> GetProjectWithMembersAsync(Guid projectId)
        {
            return await _dbContext.Projects
                .Include(project => project.ProjectMembers)
                    .ThenInclude(member => member.User)
                .Include(project => project.ProjectMembers)
                    .ThenInclude(member => member.ProjectRole)
                    .ThenInclude(role => role.Permissions)
                .FirstOrDefaultAsync(project => project.Id == projectId);
        }

        public async Task<Project?> GetProjectAsync(Guid projectId)
        {
            return await _dbContext.Projects.Include(project => project.ProjectMembers).ThenInclude(member => member.User).FirstOrDefaultAsync(project => project.Id == projectId);
        }

        public async Task<List<Guid>?> GetAllProjectIdsForUserAsync(Guid userId)
        {
            return await _dbContext.ProjectMembers.Where(member => member.UserId == userId).Select(member => member.ProjectId).ToListAsync();
        }

        public async Task<List<Project>> GetAllProjectsForUserAsync(List<Guid> projectIds)
        {
            return await _dbContext.Projects.Where(project => projectIds.Contains(project.Id)).Include(project => project.ProjectMembers).ThenInclude(member => member.User).Include(project => project.ProjectMembers).ThenInclude(member => member.ProjectRole).ThenInclude(role => role.Permissions).ToListAsync();
        }

        public async Task<List<ProjectMember>?> GetAllProjectMembers(Guid projectId)
        {
            return await _dbContext.ProjectMembers.Where(projectMember => projectMember.ProjectId == projectId).ToListAsync();
        }
    }
}
