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
    [Route("api/v1/projects/{projectId}/task-types")]
    public class TaskTypesController : BaseController
    {
        private readonly ITaskTypesService _taskTypesService;
        
        public TaskTypesController(ITaskTypesService taskTypesService)
        {
            _taskTypesService = taskTypesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskType(Guid projectId, CreateTaskTypeRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<TaskType?> response = await _taskTypesService.CreateTaskTypeAsync(userId, projectId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Common.InternalError });
            }
        }

        [HttpDelete("{typeId}")]
        public async Task<IActionResult> DeleteTaskType(DeleteTaskTypeRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse response = await _taskTypesService.DeleteTaskTypeAsync(userId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Common.InternalError });
            }
        }

        [HttpPatch("{typeId}")]
        public async Task<IActionResult> ChangeTaskTypeTitle(Guid projectId, int typeId, ChangeTaskTypeTitleRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<TaskType?> response = await _taskTypesService.ChangeTaskTypeTitleAsync(userId, projectId, typeId, request);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Common.InternalError });
            }
        }

        [HttpGet("{typeId}")]
        public async Task<IActionResult> GetTypeById(Guid projectId, int typeId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<TaskType?> response = await _taskTypesService.GetTaskTypeByIdAsync(userId, projectId, typeId);
                return HandleResponse(response);
            }
            catch
            {
                return StatusCode(500, new { Message = ResponseMessage.Common.InternalError });
            }
        }
    }
}
