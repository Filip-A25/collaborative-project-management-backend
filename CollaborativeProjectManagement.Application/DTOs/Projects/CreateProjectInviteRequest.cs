using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectInviteRequest
    {
        [Required]
        public required string InvitedUserEmail { get; set; }
        [Required]
        public required int RoleId { get; set; }
    }
}
