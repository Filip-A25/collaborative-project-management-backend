using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Common;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IProjectsService
    {
        Task<ServiceResponse<ProjectDTO?>> CreateProjectAsync(Guid userId, CreateProjectRequest request);
        Task<ServiceResponse> DeleteProjectAsync(Guid projectId, Guid userId);
    }
}