using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Interfaces.Projects
{
    public interface IProjectRolesRepository
    {
        Task<ProjectRole> CreateProjectRoleAsync(ProjectRole projectRole);
        Task<List<ProjectRole>> CreateProjectRolesAsync(List<ProjectRole> projectRoles);
        Task AddRolePermissionsAsync(List<RolePermission> rolePermissionList);
        Task<List<PermissionEntity>?> GetProjectMemberRolePermissionsAsync(Guid projectId, Guid userId);
        Task DeleteProjectRolesAsync(List<int> projectRoleIds, Guid projectId);
        Task<ProjectRole?> GetProjectRoleWithPermissionsAsync(Guid projectId, int projectRoleId);
    }
}