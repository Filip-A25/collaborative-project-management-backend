using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Infrastructure.Repositories
{
    public class ProjectsRepository: IProjectsRepository
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
            return await _dbContext.Projects.FindAsync(projectId);
        }
    }
}
