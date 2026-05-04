using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Application.Common;

namespace CollaborativeProjectManagement.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse> RegisterUserAsync(RegisterRequest request);
        Task<ServiceResponse<AuthResponseDTO?>> LoginUserAsync(LoginRequest request);
    }
}
