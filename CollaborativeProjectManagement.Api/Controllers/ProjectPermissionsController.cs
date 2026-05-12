using Microsoft.AspNetCore.Mvc;
using CollaborativeProjectManagement.Api.Controllers.Common;
using Microsoft.AspNetCore.Authorization;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/project-permissions")]
    public class ProjectPermissionsController: BaseController
    {

    }
}
