using System.Security.Claims;
using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/permissions")]
    public class PermissionsController : BaseController
    {
        private readonly IPermissionsService _permissionsService;

        public PermissionsController(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<List<PermissionEntity>?> response = await _permissionsService.GetAllPermissionsAsync(userId);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        } 
    }
}