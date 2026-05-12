using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class DeleteProjectRolesRequest
    {
        [Required]
        public List<int> ProjectRoleIds { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
    }
}
