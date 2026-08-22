using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services.Projects
{
    public class ProjectAuthorizationService : IProjectAuthorizationService
    {
        private readonly IProjectRolesRepository _projectRolesRepository;

        public ProjectAuthorizationService(IProjectRolesRepository projectRolesRepository)
        {
            _projectRolesRepository = projectRolesRepository;
        }

        public async Task<bool> CheckIfUserHasSufficientPermissionsAsync(Guid projectId, Guid userId, Permission permissionId)
        {
            List<PermissionEntity>? userPermissions = await _projectRolesRepository.GetProjectMemberRolePermissionsAsync(projectId, userId);
            
            if (userPermissions == null) return false;
            bool userHasSufficientPermissions = userPermissions.Any(permission => permission.Id == (int)permissionId);

            return userHasSufficientPermissions;
        }
    }
}
