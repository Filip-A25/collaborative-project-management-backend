using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Application.DTOs.Projects
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CreatorId { get; set; }
        public required string? Description { get; set; }
        public required string Status { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public required decimal CompletionPercentage { get; set; }
        public double? BudgetAmount { get; set; }
        public required string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required ICollection<ProjectMemberDTO> ProjectMembers { get; set; }

        public static ProjectDTO FromEntity(Project project) => new()
        {
            Id = project.Id,
            Name = project.Name,
            CreatorId = project.CreatorId,
            Description = project.Description,
            Status = project.Status.ToString(),
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CompletionPercentage = project.CompletionPercentage,
            BudgetAmount = project.BudgetAmount,
            Currency = project.Currency,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            ProjectMembers = project.ProjectMembers.Select(member => ProjectMemberDTO.FromEntity(member)).ToList()
        };
    }
}
