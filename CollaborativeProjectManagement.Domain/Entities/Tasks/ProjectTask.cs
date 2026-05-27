using System.ComponentModel.DataAnnotations;
using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class ProjectTask
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid CreatorId { get; set; }
        public Guid AssignedTo { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int? TypeId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User Creator { get; set; }
        public User AssignedUser { get; set; }
        public TaskType? Type { get; set; }

        public ProjectTask(Guid projectId, string title, string? description, Guid creatorId, Guid assignedTo, int priority, int status, int? type, DateOnly? startDate, DateOnly? dueDate)
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
