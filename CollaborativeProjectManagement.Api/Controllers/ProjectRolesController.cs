using System.Security.Claims;
using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/project-roles")]
    public class ProjectRolesController : BaseController
    {
        private readonly IProjectRolesService _projectRolesService;

        public ProjectRolesController(IProjectRolesService projectRolesService)
        {
            _projectRolesService = projectRolesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectRole(CreateProjectRoleRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectRoleDTO?> response = await _projectRolesService.CreateProjectRoleAsync(userId, request);
                return HandleResponse<ProjectRoleDTO?>(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProjectRoles(DeleteProjectRolesRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectRolesService.DeleteProjectRolesAsync(request.ProjectRoleIds, request.ProjectId, userId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpPost("{roleId}/permissions")]
        public async Task<IActionResult> AddProjectRolePermissions([FromRoute] int roleId, [FromBody] AddProjectRolePermissionsRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectRolesService.AddProjectRolePermissionsAsync(userId, roleId, request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }
    }
}
