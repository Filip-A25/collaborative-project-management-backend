using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services.Projects
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public PermissionsService(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<ServiceResponse<List<PermissionEntity>?>> GetAllPermissionsAsync(Guid userId)
        {
            List<PermissionEntity>? permissions = await _permissionsRepository.GetAllPermissionsAsync();
            return ServiceResponse<List<PermissionEntity>?>.Ok(permissions, null);
        }
    }
}