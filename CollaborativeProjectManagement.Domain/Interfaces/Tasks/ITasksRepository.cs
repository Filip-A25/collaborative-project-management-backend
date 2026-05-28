using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Domain.Interfaces.Tasks
{
    public interface ITasksRepository
    {
        Task<ProjectTask> CreateTaskAsync(ProjectTask task);
        Task DeleteTaskAsync(Guid projectId, Guid taskId);
        Task<List<ProjectTask>?> GetAllProjectTasks(Guid projectId);
        Task<ProjectTask?> GetProjectByIdAsync(Guid projectId, Guid taskId);
        Task<TaskType> CreateTaskTypeAsync(TaskType type);
        Task DeleteTaskTypeAsync(Guid projectId, int typeId);
    }
}
