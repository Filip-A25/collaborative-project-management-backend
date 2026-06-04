using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.Interfaces.Tasks
{
    public interface ITaskTypesService
    {
        Task<ServiceResponse<TaskType?>> CreateTaskTypeAsync(Guid userId, Guid projectId, CreateTaskTypeRequest request);
        Task<ServiceResponse> DeleteTaskTypeAsync(Guid userId, DeleteTaskTypeRequest request);
        Task<ServiceResponse<TaskType?>> ChangeTaskTypeTitleAsync(Guid userId, Guid projectId, int typeId, ChangeTaskTypeTitleRequest request);
        Task<ServiceResponse<TaskType?>> GetTaskTypeByIdAsync(Guid userId, Guid projectId, int typeId);
    }
}
