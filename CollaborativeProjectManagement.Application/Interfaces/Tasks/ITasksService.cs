using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;

namespace CollaborativeProjectManagement.Application.Interfaces.Tasks
{
    public interface ITasksService
    {
        Task<ServiceResponse<ProjectTaskDTO?>> CreateTaskAsync(Guid userId, Guid projectId, CreateProjectTaskRequest request);
        Task<ServiceResponse> DeleteTaskAsync(Guid userId, DeleteTaskRequest request);
        Task<ServiceResponse<List<ProjectTaskDTO>?>> GetAllProjectTasks(Guid userId, Guid projectId);
    }
}
