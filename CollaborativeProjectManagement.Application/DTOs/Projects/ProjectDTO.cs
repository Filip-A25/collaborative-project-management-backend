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
        public double? BudgetAmount { get; set; }
        public required string Currency { get; set; }
        public DateOnly? CompletedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required ICollection<ProjectMemberDTO> ProjectMembers { get; set; }
        public required ICollection<ProjectRoleDTO> Roles { get; set; }

        public static ProjectDTO FromEntity(Project project) => new()
        {
            Id = project.Id,
            Name = project.Name,
            CreatorId = project.CreatorId,
            Description = project.Description,
            Status = project.Status.ToString(),
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            BudgetAmount = project.BudgetAmount,
            Currency = project.Currency,
            CompletedDate = project.CompletedDate,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            ProjectMembers = project.ProjectMembers.Select(member => ProjectMemberDTO.FromEntity(member)).ToList(),
            Roles = project.ProjectRoles.Select(role => ProjectRoleDTO.FromEntity(role)).ToList()
        };
    }
}
