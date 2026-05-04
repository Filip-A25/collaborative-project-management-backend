using CollaborativeProjectManagement.Application.DTOs.Auth;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.Interfaces.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;

namespace CollaborativeProjectManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> RegisterUserAsync(RegisterRequest request)
        {
            if (await _userRepository.CheckIfEmailExistsAsync(request.Email))
            {
                return new ServiceResponse
                {
                    Success = false,
                    StatusCode = 409,
                    Message = "An account with this email already exists."
                };
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(request.FirstName, request.LastName, request.Username, request.Email, hashedPassword, UserRole.Member);

            await _userRepository.CreateAsync(user);

            UserDTO userData = new UserDTO
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };

            string authToken = GenerateJWTToken(user);

            return new ServiceResponse
            {
                Success = true,
                StatusCode = 200,
                Message = "User has been successfully registered."
            };
        }

        public async Task<ServiceResponse<AuthResponseDTO?>> LoginUserAsync(LoginRequest request)
        {
            bool isEmailLogin = Regex.IsMatch(request.EmailOrUsername, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            User? requestedUser = null;

            if (isEmailLogin)
            {
                requestedUser = await _userRepository.GetUserByEmailAsync(request.EmailOrUsername);
            } else
            {
                requestedUser = await _userRepository.GetUserByUsernameAsync(request.EmailOrUsername);
            }

            if (requestedUser == null)
            {
                return new ServiceResponse<AuthResponseDTO?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found."
                };
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, requestedUser.PasswordHash);

            if (!isPasswordCorrect)
            {
                return new ServiceResponse<AuthResponseDTO?>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Incorrect password has been entered."
                };
            }

            string authToken = GenerateJWTToken(requestedUser);
            UserDTO user = new UserDTO
            {
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

            return new ServiceResponse<AuthResponseDTO?>
            {
                Success = true,
                StatusCode = 200,
                Message = "User has successfully logged in.",
                Data = authResult
            };
        }       

        private string GenerateJWTToken(User user)
        {
            string jwtSecretKey = _configuration["Jwt:Key"];
            string jwtIssuer = _configuration["Jwt:Issuer"];
            string jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtSecretKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new Exception("Something went wrong while trying to register.");
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
