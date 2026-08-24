using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Application.DTOs.Auth;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class ProjectInviteDTO
    {
        public required int Id { get; set; }
        public Guid ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public Guid InvitedUserId { get; set; }
        public int InvitedUserRoleId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDTO? InviterUser { get; set; }

        public static ProjectInviteDTO FromEntity(ProjectInvite invite) => new()
        {
            Id = invite.Id,
            ProjectId = invite.ProjectId,
            ProjectName = invite.Project.Name,
            InvitedUserId = invite.InvitedUserId,
            InvitedUserRoleId = invite.InvitedUserRoleId,
            ExpiresAt = invite.ExpiresAt,
            InviterUser = UserDTO.FromEntity(invite.InviterUser)
        };
    }
}
