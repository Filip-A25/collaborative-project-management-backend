using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IProjectsService
    {
        Task<ServiceResponse<ProjectDTO?>> CreateProjectAsync(Guid userId, CreateProjectRequest request);
        Task<ServiceResponse> DeleteProjectAsync(Guid projectId, Guid userId);
        Task<ServiceResponse<ProjectDTO?>> GetProjectAsync(Guid projectId, Guid userId);
        Task<ServiceResponse<List<ProjectDTO>?>> GetAllProjectsForUserAsync(Guid userId);
        Task<ServiceResponse> RemoveMemberFromProjectAsync(Guid userId, Guid projectId, int memberId);
    }
}
