using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Interfaces.Projects
{
    public interface IProjectsRepository
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<ProjectRole> CreateProjectRoleAsync(ProjectRole projectRole);
        Task DeleteProjectAsync(Guid projectId);
        Task AddMemberToProjectAsync(ProjectMember member);
        Task<Project?> GetProjectWithMembersAsync(Guid projectId);
        Task<Project?> GetProjectAsync(Guid projectId);
        Task<List<Guid>?> GetAllProjectIdsForUserAsync(Guid userId);
        Task<List<Project>> GetAllProjectsForUserAsync(List<Guid> projectIds);
    }
}
