using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CreateProjectTaskRequest
    {
        [FromRoute]
        public Guid ProjectId { get; set; }
        [Required]
        [MaxLength(300)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public Guid CreatorId { get; set; }
        public Guid AssignedTo { get; set; }
        [Required]
        public int Priority { get; set; }
        public int Status { get; set; }
        public int? Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
    }
}
