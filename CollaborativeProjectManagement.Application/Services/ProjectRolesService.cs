using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services
{
    public class ProjectRolesService: IProjectRolesService
    {
        private readonly IProjectRolesRepository _projectRolesRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;

        private const string CreatorRoleName = "Creator";
        private const string CreatorRoleDefaultColorHex = "cf233a";

        public ProjectRolesService(IProjectRolesRepository projectRolesRepository, IProjectAuthorizationService projectAuthorizationService)
        {
            _projectRolesRepository = projectRolesRepository;
            _projectAuthorizationService = projectAuthorizationService;
        }

        public async Task<ServiceResponse<ProjectRoleDTO?>> CreateProjectRoleAsync(Guid userId, CreateProjectRoleRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId, userId, Permission.ManageRoles);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectRoleDTO?>.Forbidden(null, ResponseMessage.ProjectRoles.RolesManageError);

            var newProjectRole = new ProjectRole
            {
                ProjectId = request.ProjectId,
                Name = request.Name,
                Color = request.Color
            };

            await _projectRolesRepository.CreateProjectRoleAsync(newProjectRole);
            newProjectRole = await _projectRolesRepository.GetProjectRoleWithPermissionsAsync(request.ProjectId, newProjectRole.Id);

            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                await AssignPermissionsToRole(newProjectRole.Id, request.PermissionIds);
            }

            ProjectRoleDTO projectRoleDto = ProjectRoleDTO.FromEntity(newProjectRole);

            return ServiceResponse<ProjectRoleDTO?>.Created(projectRoleDto, null);
        }

        public async Task<ServiceResponse> AddProjectRolePermissionsAsync(Guid userId, int roleId, AddProjectRolePermissionsRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId, userId, Permission.ManageRoles);
            if (!userHasSufficientPermissions) return ServiceResponse.Forbidden(ResponseMessage.ProjectRoles.RolesManageError);

            await AssignPermissionsToRole(roleId, request.PermissionIds);

            return ServiceResponse.Ok(ResponseMessage.ProjectRoles.AddPermissionsSuccess);
        }

        public async Task<ServiceResponse> DeleteProjectRolesAsync(List<int> projectRoleIds, Guid projectId, Guid userId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageRoles);
            if (!userHasSufficientPermissions) return ServiceResponse.Forbidden(ResponseMessage.ProjectRoles.RolesManageError);

            await _projectRolesRepository.DeleteProjectRolesAsync(projectRoleIds, projectId);

            return ServiceResponse.NoContent(ResponseMessage.ProjectRoles.DeleteBatchSuccess);
        }

        public async Task<ProjectRole> AddCreatorRole(Guid projectId, Guid creatorId)
        {
            ProjectRole creatorRole = new ProjectRole
            {
                ProjectId = projectId,
                Name = CreatorRoleName,
                Color = CreatorRoleDefaultColorHex
            };

            await _projectRolesRepository.CreateProjectRoleAsync(creatorRole);

            List<RolePermission> rolePermissions = Enum.GetValues<Permission>().Select(permission => new RolePermission
            {
                ProjectRoleId = creatorRole.Id,
                PermissionId = (int)permission
            }).ToList();

            await _projectRolesRepository.AddRolePermissionsAsync(rolePermissions);

            return creatorRole;
        }

        private async Task AssignPermissionsToRole(int roleId, List<int> permissionIds)
        {
            List<RolePermission> rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                ProjectRoleId = roleId,
                PermissionId = permissionId
            }).ToList();

            await _projectRolesRepository.AddRolePermissionsAsync(rolePermissions);
        }
    }
}
