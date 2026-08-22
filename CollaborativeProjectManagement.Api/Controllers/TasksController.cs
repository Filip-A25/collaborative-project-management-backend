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
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.CreateTaskAsync(projectId, userId, request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _tasksService.DeleteTaskAsync(userId, projectId, taskId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectTasks(Guid projectId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<ProjectTaskDTO>?> response = await _tasksService.GetAllProjectTasksAsync(userId, projectId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid projectId, Guid taskId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.GetTaskByIdAsync(userId, projectId, taskId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpPatch("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, UpdateTaskRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.UpdateTaskAsync(userId, projectId, taskId, request);
                return HandleResponse(response);
            }
            catch
            {   
                return HandleInternalError();
            }
        }
    }
}
