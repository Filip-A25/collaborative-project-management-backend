using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Auth;

namespace CollaborativeProjectManagement.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse> RegisterUserAsync(RegisterRequest request);
        Task<ServiceResponse<AuthResponseDTO?>> LoginUserAsync(LoginRequest request);
    }
}
