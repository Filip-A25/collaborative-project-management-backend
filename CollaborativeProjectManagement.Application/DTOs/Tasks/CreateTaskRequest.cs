using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CreateProjectTaskRequest
    {
        [Required]
        [MaxLength(300)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int? AssignedTo { get; set; }
        [Required]
        public int Priority { get; set; }
        public int Status { get; set; }
        public int? Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
    }
}
