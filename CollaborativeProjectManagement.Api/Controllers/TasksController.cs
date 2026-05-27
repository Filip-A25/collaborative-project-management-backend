using System.Security.Claims;
using Azure.Core;
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
    [Route("api/v1/projects/{projectId}/tasks")]
    public class TasksController : BaseController
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid projectId, CreateProjectTaskRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.CreateTaskAsync(userId, projectId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.InternalCreateError });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(DeleteTaskRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _tasksService.DeleteTaskAsync(userId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.InternalDeleteError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectTasks(Guid projectId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<ProjectTaskDTO>?> response = await _tasksService.GetAllProjectTasks(userId, projectId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.TasksManageError });
            }
        }
    }
}
