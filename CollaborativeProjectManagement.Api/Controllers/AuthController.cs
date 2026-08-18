using CollaborativeProjectManagement.Api.Controllers.Common;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPatch]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<UserDTO?> response = await _authService.UpdateUserAsync(userId, request);
                return HandleResponse<UserDTO?>(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ServiceResponse<UserDTO?> response = await _authService.GetUserAsync(userId);
                return HandleResponse<UserDTO?>(response);
            }
            catch
            {
                return HandleInternalError();
            }
        }
    }
}
