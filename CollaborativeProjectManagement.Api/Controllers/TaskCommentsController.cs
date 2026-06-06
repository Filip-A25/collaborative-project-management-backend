using System.Security.Claims;
using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Application.Interfaces.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/projects/{projectId}/tasks/{taskId}/comments")]
    public class TaskCommentsController : BaseController
    {
        private readonly ITaskCommentsService _taskCommentsService;

        public TaskCommentsController(ITaskCommentsService taskCommentsService)
        {
            _taskCommentsService = taskCommentsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskComment(Guid projectId, Guid taskId, CreateTaskCommentRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<CommentDTO?> response = await _taskCommentsService.CreateTaskCommentAsync(userId, projectId, taskId, request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteTaskComment(Guid projectId, Guid taskId, int commentId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _taskCommentsService.DeleteTaskCommentAsync(userId, projectId, taskId, commentId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpPatch("{commentId}")]
        public async Task<IActionResult> UpdateTaskComment(Guid projectId, Guid taskId, int commentId, UpdateTaskCommentRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<CommentDTO?> response = await _taskCommentsService.UpdateTaskCommentAsync(userId, projectId, taskId, commentId, request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskComments(Guid projectId, Guid taskId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<CommentDTO>?> response = await _taskCommentsService.GetAllTaskCommentsAsync(userId, projectId, taskId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }
    }
}
