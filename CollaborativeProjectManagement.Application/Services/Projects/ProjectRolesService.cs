using System.Diagnostics.Eventing.Reader;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services.Projects
{
    public class ProjectRolesService : IProjectRolesService
    {
        private readonly IProjectRolesRepository _projectRolesRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IProjectsRepository _projectsRepository;

        private const string CreatorRoleName = "Creator";
        private const string DefaultCreatorRoleColor = "blue";

        public ProjectRolesService(IProjectRolesRepository projectRolesRepository, IProjectAuthorizationService projectAuthorizationService, IProjectsRepository projectsRepository)
        {
            _projectRolesRepository = projectRolesRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _projectsRepository = projectsRepository;
        }

        public async Task<ServiceResponse<ProjectRoleDTO?>> CreateProjectRoleAsync(Guid userId, CreateProjectRoleRequest request)
        {
            if (request.ProjectId == null) return ServiceResponse<ProjectRoleDTO?>.BadRequest(null, ResponseMessage.ProjectRoles.ProjectIdMissing);

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId.Value, userId, Permission.ManageRoles);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectRoleDTO?>.Forbidden(null, ResponseMessage.ProjectRoles.RolesManageError);

            var newProjectRole = new ProjectRole
            {
                ProjectId = request.ProjectId.Value,
                Name = request.Name,
                Color = request.Color
            };

            await _projectRolesRepository.CreateProjectRoleAsync(newProjectRole);
            newProjectRole = await _projectRolesRepository.GetProjectRoleWithPermissionsAsync(request.ProjectId.Value, newProjectRole.Id);

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

            ProjectRole? targetRole = await _projectRolesRepository.GetProjectRoleAsync(request.ProjectId, roleId);
            if (targetRole == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.ProjectRoles.RoleNotFound);
            }

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

        private async Task<ProjectRole> AddCreatorRole(Guid projectId, Guid creatorId)
        {
            ProjectRole creatorRole = new ProjectRole
            {
                ProjectId = projectId,
                Name = CreatorRoleName,
                Color = DefaultCreatorRoleColor,
                IsCreatorRole = true
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

        public async Task AssignCreatorRoleToUser(Guid projectId, Guid userId)
        {
            ProjectRole creatorRole = await AddCreatorRole(projectId, userId);
            ProjectMember newMember = new ProjectMember(userId, projectId, creatorRole.Id);

            await _projectsRepository.AddMemberToProjectAsync(newMember);
        }

        public async Task AssignPermissionsToRole(int roleId, List<int> permissionIds)
        {
            List<RolePermission> rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                ProjectRoleId = roleId,
                PermissionId = permissionId
            }).ToList();

            await _projectRolesRepository.AddRolePermissionsAsync(rolePermissions);
        }

        public async Task BulkUpdateProjectRoles(Guid projectId, List<UpdateProjectRoleRequest>? roleRequests)
        {
            List<ProjectRole>? dbRoles = await _projectRolesRepository.GetAllRolesForProject(projectId);
            Dictionary<int, ProjectRole> dbRolesById = dbRoles.ToDictionary(role => role.Id);           

            foreach (UpdateProjectRoleRequest roleRequest in roleRequests)
            {
                int roleId;
                List<int> permissionsToAdd = [];
                List<int> requestPermissionsId = roleRequest.Permissions != null ? roleRequest.Permissions.Select(permission => permission.Id).ToList() : [];

                if (dbRolesById.TryGetValue(roleRequest.Id, out ProjectRole currentRole))
                {
                    roleId = currentRole.Id;

                    currentRole.Id = roleRequest.Id;
                    currentRole.Name = roleRequest.Name;
                    currentRole.Color = roleRequest.Color;

                    List<int>? currentRolePermissions = currentRole.Permissions?.Select(permission => permission.Id).ToList();
                    bool doesRoleHaveExistingPermissions = currentRolePermissions != null && currentRolePermissions.Any();

                    if (roleRequest.Permissions != null)
                    {
                        if (doesRoleHaveExistingPermissions)
                        {
                            List<int> permissionsForRemoval = currentRolePermissions.Except(requestPermissionsId).ToList();
                            await _projectRolesRepository.UnassignPermissionsFromRole(currentRole.Id, permissionsForRemoval);                            
                        }

                        permissionsToAdd = requestPermissionsId.Except(currentRolePermissions).ToList();
                    }
                } else
                {

                    ProjectRole newProjectRole = new ProjectRole
                    {
                        ProjectId = projectId,
                        Name = roleRequest.Name,
                        Color = roleRequest.Color
                    };

                    ProjectRole newRole = await _projectRolesRepository.CreateProjectRoleAsync(newProjectRole);
                    roleId = newRole.Id;
                
                    permissionsToAdd = requestPermissionsId ?? [];
                }

                if (permissionsToAdd.Any())
                {
                    List<RolePermission> newRolePermissions = permissionsToAdd.Select(permissionId => new RolePermission
                    {
                        ProjectRoleId = roleId,
                        PermissionId = permissionId
                    }).ToList();

                    _projectRolesRepository.AddRolePermissions(newRolePermissions);
                }

                await _projectRolesRepository.UpdateProjectRolesAsync();
            }             
        }
    }
}
