using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class ProjectRoleDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
        public bool IsCreatorRole { get; set; }
        public ICollection<PermissionEntity>? Permissions { get; set; }

        public static ProjectRoleDTO FromEntity(ProjectRole role) => new()
        {
            Id = role.Id,
            Name = role.Name,
            Color = role.Color,
            IsCreatorRole = role.IsCreatorRole,
            Permissions = role.Permissions
        };
    }
}
