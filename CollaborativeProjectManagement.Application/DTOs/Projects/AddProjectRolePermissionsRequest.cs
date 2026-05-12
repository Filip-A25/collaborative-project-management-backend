using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class AddProjectRolePermissionsRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public List<int> PermissionIds { get; set; } = [];
    }
}
