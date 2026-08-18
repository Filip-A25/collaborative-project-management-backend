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
    [Route("api/v1/projects")]
    public class ProjectsController : BaseController
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectDTO?> response = await _projectsService.CreateProjectAsync(userId, request);
                return HandleResponse<ProjectDTO?>(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectsService.DeleteProjectAsync(projectId, userId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectDTO?> response = await _projectsService.GetProjectAsync(projectId, userId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectsForUser()
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<ProjectDTO>?> response = await _projectsService.GetAllProjectsForUserAsync(userId);
                return HandleResponse(response);
            }
            catch
            {   
                return HandleInternalError();
            }
        }

        [HttpDelete("{projectId}/members/{memberId}")]
        public async Task<IActionResult> RemoveMemberFromProject(Guid projectId, int memberId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectsService.RemoveMemberFromProjectAsync(userId, projectId, memberId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpPatch("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, UpdateProjectRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectsService.UpdateProjectAsync(userId, projectId, request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }
    }
}
