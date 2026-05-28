using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class DeleteTaskTypeRequest
    {
        [FromRoute]
        public Guid ProjectId { get; set; }
        public int TypeId { get; set; }
    }
}
