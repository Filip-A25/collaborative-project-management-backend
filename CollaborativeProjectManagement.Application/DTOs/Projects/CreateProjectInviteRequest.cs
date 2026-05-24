using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectInviteRequest
    {
        [Required]
        public Guid InvitedUserId { get; set; }
        [Required]
        public required int RoleId { get; set; }
        [Required]
        public required DateTime ExpiresAt { get; set; }
    }
}
