using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Domain.Interfaces.Tasks
{
    public interface ITaskTypesRepository
    {
        Task<TaskType> CreateTaskTypeAsync(TaskType type);
        Task DeleteTaskTypeAsync(Guid projectId, int typeId);
        Task<TaskType?> ChangeTaskTypeTitleAsync(Guid projectId, int typeId, string title);
        Task<TaskType?> GetTaskTypeByIdAsync(Guid projectId, int typeId);
        Task<List<TaskType>> GetTaskTypesAsync(Guid projectId);
    }
}
