using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Interfaces.Projects
{
    public interface IProjectsRepository
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<ProjectRole> CreateProjectRoleAsync(ProjectRole projectRole);
        Task DeleteProjectAsync(Guid projectId);
        Task AddMemberToProjectAsync(ProjectMember member);
        Task<Project?> GetProjectWithFullMembersAsync(Guid projectId);
        Task<Project?> GetProjectWithMembersAsync(Guid projectId);
        Task<Project?> GetProjectAsync(Guid projectId);
        Task<List<Guid>> GetAllProjectIdsForUserAsync(Guid userId);
        Task<List<Project>> GetAllProjectsForUserAsync(List<Guid> projectIds);
        Task<List<ProjectMember>?> GetAllProjectMembersAsync(Guid projectId);
        Task<ProjectMember?> GetProjectMemberAsync(Guid projectId, Guid userId);
        Task<ProjectMember?> GetProjectMemberByIdAsync(Guid projectId, int memberId);
        Task RemoveMemberFromProjectAsync(Guid projectId, int memberId);
        Task UpdateProjectAsync();
        Task<string?> GetProjectNameAsync(Guid projectId);
    }
}
