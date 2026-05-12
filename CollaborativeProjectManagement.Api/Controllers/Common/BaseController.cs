using Microsoft.AspNetCore.Mvc;
using CollaborativeProjectManagement.Application.Common;

namespace CollaborativeProjectManagement.Api.Controllers.Common
{
    public class BaseController: ControllerBase
    {
        protected IActionResult HandleResponse(ServiceResponse response)
        {
            return StatusCode((int)response.StatusCode, new
            {
                response.Success,
                response.Message
            });
        }
        
        protected IActionResult HandleResponse<T>(ServiceResponse<T> response)
        {
            return StatusCode((int)response.StatusCode, new
            {
                response.Success,
                response.Data,
                response.Message 
            });
        }
    }
}
