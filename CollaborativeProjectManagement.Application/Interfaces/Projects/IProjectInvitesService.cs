using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IProjectInvitesService
    {
        Task<ServiceResponse> CreateProjectInviteAsync(Guid userId, Guid projectId, CreateProjectInviteRequest request);
        Task<ServiceResponse> DeleteProjectInviteAsync(Guid userId, Guid projectId, int inviteId);
        Task<ServiceResponse> AcceptProjectInviteAsync(Guid userId, Guid projectId, int inviteId);
        Task<ServiceResponse<List<ProjectInvite>?>> GetAllUserInvitesAsync(Guid userId);
        Task<ServiceResponse<List<ProjectInvite>?>> GetAllProjectsInvitesAsync(Guid userId, Guid projectId);
    }
}
