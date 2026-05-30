using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.Interfaces.Tasks
{
    public interface ITasksService
    {
        Task<ServiceResponse<ProjectTaskDTO?>> CreateTaskAsync(Guid userId, CreateProjectTaskRequest request);
        Task<ServiceResponse> DeleteTaskAsync(Guid userId, DeleteTaskRequest request);
        Task<ServiceResponse<List<ProjectTaskDTO>?>> GetAllProjectTasksAsync(Guid userId, Guid projectId);
        Task<ServiceResponse<ProjectTaskDTO?>> GetTaskByIdAsync(Guid userId, Guid projectId, Guid taskId);
    }
}
