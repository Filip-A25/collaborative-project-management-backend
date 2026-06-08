using System.ComponentModel.DataAnnotations;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class ProjectTask
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? CreatorId { get; set; }
        public int? AssignedTo { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        public int? TypeId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ProjectMember Creator { get; set; }
        public ProjectMember AssignedUser { get; set; }
        public TaskType? Type { get; set; }

        public ProjectTask(Guid projectId, string title, string? description, int creatorId, int? assignedTo, int priority, int status, int? type, DateOnly? startDate, DateOnly? dueDate)
        {
            Id = Guid.NewGuid();
            ProjectId = projectId;
            Title = title;
            Description = description;
            CreatorId = creatorId;
            AssignedTo = assignedTo;
            Priority = (TaskPriority) priority;
            Status = (TaskStatus) status;
            TypeId = type;
            StartDate = startDate;
            DueDate = dueDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private ProjectTask() { }
    }
}
