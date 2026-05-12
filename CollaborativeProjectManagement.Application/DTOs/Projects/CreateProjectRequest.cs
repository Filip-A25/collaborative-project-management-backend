using CollaborativeProjectManagement.Domain.Entities.Projects;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class CreateProjectRequest
    {
        [MaxLength(120)]
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public required DateOnly StartDate { get; set; }
        [Required]
        public required DateOnly EndDate { get; set; }
        [MaxLength(3)]
        [Required]
        public required string Currency { get; set; }
        public double? BudgetAmount { get; set; }
        public List<ProjectRole>? Roles { get; set; }
    }
}
