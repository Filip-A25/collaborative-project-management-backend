using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public class ProjectInvite
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid InviterUserId { get; set; }
        [Required]
        public Guid InvitedUserId { get; set; }
        [Required]
        public int InvitedUserRoleId { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Project Project { get; set; }
        public User InviterUser { get; set; } 

        public ProjectInvite(Guid projectId, Guid inviterUserId, Guid invitedUserId, int invitedUserRoleId, DateTime expiresAt)
        {
            ProjectId = projectId;
            InviterUserId = inviterUserId;
            InvitedUserId = invitedUserId;
            InvitedUserRoleId = invitedUserRoleId;
            ExpiresAt = expiresAt;
            IsAccepted = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private ProjectInvite() { }
    }
}
