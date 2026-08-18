using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Application.Interfaces.Auth;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CollaborativeProjectManagement.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        private const string EMAIL_FORMAT_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> RegisterUserAsync(RegisterRequest request)
        {
            if (await _userRepository.CheckIfEmailExistsAsync(request.Email))
            {
                return ServiceResponse.Conflict(ResponseMessage.Auth.RegisterConflict);
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(request.FirstName, request.LastName, request.Username, request.Email, hashedPassword, UserRole.Member);

            await _userRepository.CreateAsync(user);

            return ServiceResponse.Ok(ResponseMessage.Auth.RegisterSuccess);
        }

        public async Task<ServiceResponse<AuthResponseDTO?>> LoginUserAsync(LoginRequest request)
        {
            bool isEmailLogin = Regex.IsMatch(request.EmailOrUsername, EMAIL_FORMAT_REGEX);

            User? requestedUser = null;

            if (isEmailLogin)
            {
                requestedUser = await _userRepository.GetUserByEmailAsync(request.EmailOrUsername);
            }
            else
            {
                requestedUser = await _userRepository.GetUserByUsernameAsync(request.EmailOrUsername);
            }

            if (requestedUser == null)
            {
                return ServiceResponse<AuthResponseDTO?>.NotFound(null, ResponseMessage.Auth.UserNotFound);
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, requestedUser.PasswordHash);

            if (!isPasswordCorrect)
            {
                return ServiceResponse<AuthResponseDTO?>.Unauthorized(null, ResponseMessage.Auth.IncorrectPassword);
            }

            string authToken = GenerateJWTToken(requestedUser);
            UserDTO user = new UserDTO
            {
                Id = requestedUser.Id,
                FirstName = requestedUser.FirstName,
                LastName = requestedUser.LastName,
                Username = requestedUser.Username,
                Email = requestedUser.Email,
                Role = requestedUser.Role
            };

            AuthResponseDTO authResult = new AuthResponseDTO
            {
                UserData = user,
                Token = authToken
            };

            return ServiceResponse<AuthResponseDTO?>.Ok(authResult, ResponseMessage.Auth.LoginSuccess);
        }

        private string? GenerateJWTToken(User user)
        {
            string jwtSecretKey = _configuration["Jwt:Secret"];
            string jwtIssuer = _configuration["Jwt:Issuer"];
            string jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtSecretKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", user.Role.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("username", user.Username),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            double tokenExpiryTime = double.TryParse(_configuration["Jwt:ExpiryMinutes"], out double value) ? value : 60.0;

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiryTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<UserDTO?>> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            User? targetUser = await _userRepository.GetUserByIdAsync(userId);
            if (targetUser == null)
            {
                return ServiceResponse<UserDTO?>.NotFound(null, ResponseMessage.Auth.UserNotFound);
            }

            if (request.Username != null && targetUser.Username != request.Username)
            {
                bool doesIdenticalUsernameExist = await _userRepository.CheckForExistingUsernameAsync(userId, request.Username);
                if (doesIdenticalUsernameExist) return ServiceResponse<UserDTO?>.Conflict(null, ResponseMessage.Auth.NonUniqueUsername);
            }

            if (request.Email != null && targetUser.Email != request.Email)
            {
                bool doesIdenticalEmailExist = await _userRepository.CheckForExistingEmailAsync(userId, request.Email);
                if (doesIdenticalEmailExist) return ServiceResponse<UserDTO?>.Conflict(null, ResponseMessage.Auth.NonUniqueEmail);
            }

            targetUser.FirstName = request.FirstName ?? targetUser.FirstName;
            targetUser.LastName = request.LastName ?? targetUser.LastName;
            targetUser.Username = request.Username ?? targetUser.Username;
            targetUser.Email = request.Email ?? targetUser.Email;

            await _userRepository.UpdateUserAsync();

            UserDTO userDto = UserDTO.FromEntity(targetUser);
            return ServiceResponse<UserDTO?>.Ok(userDto, ResponseMessage.Auth.UpdateSuccess);
        }

        public async Task<ServiceResponse<UserDTO?>> GetUserAsync(Guid userId)
        {
            User? targetUser = await _userRepository.GetUserByIdAsync(userId);
            if (targetUser == null)
            {
                return ServiceResponse<UserDTO?>.NotFound(null, ResponseMessage.Auth.UserNotFound);
            }
            
            UserDTO userDto = UserDTO.FromEntity(targetUser);            
            return ServiceResponse<UserDTO?>.Ok(userDto, null);
        }
    }
}
