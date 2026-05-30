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

            UserDTO userData = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };

            string? authToken = GenerateJWTToken(user);

            if (authToken == null) return ServiceResponse.InternalServerError(ResponseMessage.Auth.InternalRegisterError);

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
            string jwtSecretKey = _configuration["Jwt:Key"];
            string jwtIssuer = _configuration["Jwt:Issuer"];
            string jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtSecretKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

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
    }
}
