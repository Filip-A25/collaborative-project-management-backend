using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class UpdateProjectRequest
    {
        [MaxLength(120)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [MaxLength(3)]
        public string? Currency { get; set; }
        public double? BudgetAmount { get; set; }
        public required string Status { get; set; }
        public List<UpdateProjectRoleRequest>? Roles { get; set; }
    }
}
