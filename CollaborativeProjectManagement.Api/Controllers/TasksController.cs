using System.Security.Claims;
using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Application.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
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
        public async Task<IActionResult> CreateTask(CreateProjectTaskRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.CreateTaskAsync(userId, request);
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
                ServiceResponse<List<ProjectTaskDTO>?> response = await _tasksService.GetAllProjectTasksAsync(userId, projectId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.TasksManageError });
            }
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetProjectById(Guid projectId, Guid taskId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<ProjectTaskDTO?> response = await _tasksService.GetTaskByIdAsync(userId, projectId, taskId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.InternalFetchError });
            }
        }

        [HttpPost("{taskId}/types")]
        public async Task<IActionResult> CreateTaskType(CreateTaskTypeRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<TaskType?> response = await _tasksService.CreateTaskTypeAsync(userId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Tasks.InternalTypeCreateError });
            }
        }

        [HttpPost("{taskId}/types/{typeId}")]
        public async Task<IActionResult> DeleteTaskType(DeleteTaskTypeRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _tasksService.DeleteTaskTypeAsync(userId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = "" });
            }
        }
    }
}
