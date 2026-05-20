using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
