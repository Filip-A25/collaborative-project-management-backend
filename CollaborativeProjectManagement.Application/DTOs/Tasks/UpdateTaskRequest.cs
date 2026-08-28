namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssignedTo { get; set; }
        public required string Priority { get; set; }
        public required string Status { get; set; }
        public int? Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
    }
}
