using Microsoft.AspNetCore.Mvc;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Api.Controllers.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
                return StatusCode(500, new { message = "Something went wrong while creating a project." });
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
            } catch
            {
                return StatusCode(500, new { Message = "Something went wrong while trying to delete the project." });
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
            } catch
            {
                return StatusCode(500, new { Message = "Something went wrong while trying to fetch the project." });
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
            } catch
            {
                return StatusCode(500, new { Message = "Something went wrong while trying to fetch the projects." });
            }
        }
    }
}