using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            try
            {
                ServiceResponse response = await _authService.RegisterUserAsync(request);
                return HandleResponse(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginRequest request)
        {
            try
            {
                ServiceResponse<AuthResponseDTO?> response = await _authService.LoginUserAsync(request);
                return HandleResponse<AuthResponseDTO?>(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }
    }
}
