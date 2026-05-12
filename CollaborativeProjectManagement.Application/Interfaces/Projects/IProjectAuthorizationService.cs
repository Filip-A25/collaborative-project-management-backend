using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.Interfaces.Projects
{
    public interface IProjectAuthorizationService
    {
        Task<bool> CheckIfUserHasSufficientPermissionsAsync(Guid projectId, Guid userId, Permission permissionId);
    }
}
