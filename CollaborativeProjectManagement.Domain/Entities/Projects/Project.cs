using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public enum ProjectStatus
    {
        Active,
        Planning,
        OnHold,
        Completed
    }

    public class Project
    {
        public Guid Id { get; set; }
        [MaxLength(120)]
        public string Name { get; set; }
        public Guid CreatorId { get; set; }
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? CompletedDate { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal CompletionPercentage { get; set; }
        public double? BudgetAmount { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProjectMember> ProjectMembers { get; private set; }
        public ICollection<ProjectRole> ProjectRoles { get; private set; }

        public Project(string name, Guid creatorId, string? description, ProjectStatus status, DateOnly startDate, DateOnly endDate, string currency, double? budgetAmount)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatorId = creatorId;
            Description = description;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            BudgetAmount = budgetAmount;
            Currency = currency;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private Project() { }
    }
}
