using System.ComponentModel.DataAnnotations;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class    UpdateProjectRoleRequest
    {
        public int Id { get; set; }
        public Guid? ProjectId { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        public List<RolePermission>? Permissions { get; set; }
    }
}
