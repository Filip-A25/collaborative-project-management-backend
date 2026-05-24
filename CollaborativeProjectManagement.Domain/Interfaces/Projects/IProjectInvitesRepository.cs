using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Interfaces.Projects
{
    public interface IProjectInvitesRepository
    {
        Task CreateProjectInviteAsync(ProjectInvite projectInvites);
        Task DeleteProjectInviteAsync(Guid projectId, int inviteId);
        Task<ProjectInvite?> GetProjectInviteAsync(Guid projectId, int inviteId);
        Task UpdateProjectInviteToAcceptedAsync(ProjectInvite invite);
        Task<List<ProjectInvite>?> GetAllUserInvitesAsync(Guid userId);
        Task<List<ProjectInvite>?> GetAllProjectsInvitesAsync(Guid projectId);
    }
}
