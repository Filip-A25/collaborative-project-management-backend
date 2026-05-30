using Microsoft.AspNetCore.Mvc;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CreateTaskTypeRequest
    {
        public required string Title { get; set; }
    }
}
