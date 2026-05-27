namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class DeleteTaskRequest
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
    }
}
