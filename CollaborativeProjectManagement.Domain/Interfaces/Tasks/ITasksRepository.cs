using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Domain.Interfaces.Tasks
{
    public interface ITasksRepository
    {
        Task<ProjectTask> CreateTaskAsync(ProjectTask task);
        Task DeleteTaskAsync(Guid projectId, Guid taskId);
        Task<List<ProjectTask>> GetAllProjectTasksAsync(Guid projectId);
        Task<ProjectTask?> GetTaskWithUsersAsync(Guid projectId, Guid taskId);
        Task<ProjectTask?> GetTaskAsync(Guid projectId, Guid taskId);
        Task UpdateTaskAsync();
        Task<List<ProjectTask>> GetAllTasksFromMemberAsync(Guid projectId, int memberId);
        Task DeleteAllProjectTasksAsync(Guid projectId);
    }
}
