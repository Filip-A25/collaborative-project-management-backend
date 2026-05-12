using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CollaborativeProjectManagement.Application.Interfaces.Auth;
using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Api.Controllers.Common;

namespace CollaborativeProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class AuthController: BaseController
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
            } catch
            {
                return StatusCode(500, new {Message = "Something went wrong while trying to register."});
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
            } catch
            {
                return StatusCode(500, new { Message = "Something went wrong while trying to login." });
            }
        } 
    }
}
