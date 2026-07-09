using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Interfaces.Projects
{
    public interface IPermissionsRepository
    {
        Task<List<PermissionEntity>?> GetAllPermissionsAsync();
    }
}
