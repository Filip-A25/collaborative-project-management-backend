using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IProjectRolesService
    {
        Task<ServiceResponse<ProjectRoleDTO?>> CreateProjectRoleAsync(Guid userId, CreateProjectRoleRequest request);
        Task<ServiceResponse> AddProjectRolePermissionsAsync(Guid userId, int roleId, AddProjectRolePermissionsRequest request);
        Task<ServiceResponse> DeleteProjectRolesAsync(List<int> projectRoleIds, Guid projectId, Guid userId);
        Task<ProjectRole> AddCreatorRole(Guid projectId, Guid creatorId);
    }
}
