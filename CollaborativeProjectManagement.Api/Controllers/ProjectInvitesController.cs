using System.Security.Claims;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/projects/{projectId}/invites")]
    public class ProjectInvitesController : BaseController
    {
        private readonly IProjectInvitesService _projectInvitesService;

        public ProjectInvitesController(IProjectInvitesService projectInvitesService)
        {
            _projectInvitesService = projectInvitesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectInvite(Guid projectId, CreateProjectInviteRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectInvitesService.CreateProjectInviteAsync(userId, projectId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.ProjectInvites.InternalCreateError });
            }
        }

        [HttpDelete("{inviteId}")]
        public async Task<IActionResult> DeleteProjectInvite(Guid projectId, int inviteId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectInvitesService.DeleteProjectInviteAsync(userId, projectId, inviteId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.ProjectInvites.InternalDeleteError });
            }
        }

        [HttpPatch("{inviteId}/accept")]
        public async Task<IActionResult> AcceptProjectInvite(Guid projectId, int inviteId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _projectInvitesService.AcceptProjectInviteAsync(userId, projectId, inviteId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.ProjectInvites.InternalUpdateError });
            }
        }

        [HttpGet("~/api/v1/invites/me")]
        public async Task<IActionResult> GetAllUserInvites()
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<ProjectInvite>?> response = await _projectInvitesService.GetAllUserInvitesAsync(userId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.ProjectInvites.InternalFetchError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectInvites(Guid projectId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<ProjectInvite>?> response = await _projectInvitesService.GetAllProjectsInvitesAsync(userId, projectId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.ProjectInvites.InternalFetchError });
            }
        }
    }
}
