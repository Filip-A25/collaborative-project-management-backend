using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public ProjectMemberDTO? Creator { get; set; }
        public ProjectMemberDTO? AssignedTo { get; set; }
        public required string Priority { get; set; }
        public required string Status { get; set; }
        public TaskType? Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static ProjectTaskDTO FromEntity(ProjectTask task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Creator = ProjectMemberDTO.FromEntity(task.Creator),
            AssignedTo = ProjectMemberDTO.FromEntity(task.AssignedUser),
            Priority = task.Priority.ToString(),
            Status = task.Status.ToString(),
            Type = task.Type,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
