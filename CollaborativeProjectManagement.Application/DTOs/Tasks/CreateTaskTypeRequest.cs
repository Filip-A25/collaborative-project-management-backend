using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CreateTaskTypeRequest
    {
        [FromRoute]
        public Guid ProjectId { get; set; }
        public required string Title { get; set; }
    }
}
