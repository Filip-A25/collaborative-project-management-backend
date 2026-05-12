using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class ProjectRoleDTO
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
        public ICollection<PermissionEntity>? Permissions { get; set; }

        public static ProjectRoleDTO FromEntity(ProjectRole role) => new()
        {
            Name = role.Name,
            Color = role.Color,
            Permissions = role.Permissions
        };
    }
}