using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Application.Common;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IPermissionsService
    {
        Task<ServiceResponse<List<PermissionEntity>?>> GetAllPermissionsAsync(Guid userId);
    }
}
