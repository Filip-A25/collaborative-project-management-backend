using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class ProjectTaskDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required ProjectMemberDTO Creator { get; set; }
        public required ProjectMemberDTO AssignedUser { get; set; }
        public required TaskPriority Priority { get; set; }
        public required TaskStatus Status { get; set; }
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
            AssignedUser = ProjectMemberDTO.FromEntity(task.AssignedUser),
            Priority = task.Priority,
            Status = task.Status,
            Type = task.Type,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
