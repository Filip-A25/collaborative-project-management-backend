using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectRoleRequest
    {
        public Guid? ProjectId { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        public List<int>? PermissionIds { get; set; }
    }
}
