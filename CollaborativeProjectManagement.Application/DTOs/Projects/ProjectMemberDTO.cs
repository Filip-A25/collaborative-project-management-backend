using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class ProjectMemberDTO
    {
        public required int Id {get; set;}
        public required Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ProjectRoleDTO? ProjectRole { get; set; }

        public static ProjectMemberDTO FromEntity(ProjectMember member) => new()
        {
            Id = member.Id,
            UserId = member.UserId,
            Username = member.User?.Username,
            Email = member.User?.Email,
            FirstName = member.User?.FirstName,
            LastName = member.User?.LastName,
            ProjectRole = member.ProjectRole != null ? ProjectRoleDTO.FromEntity(member.ProjectRole) : null
        };
    }
}
