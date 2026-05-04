namespace CollaborativeProjectManagement.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public required UserDTO UserData { get; set; }
        public required string Token { get; set; }
    }
}
