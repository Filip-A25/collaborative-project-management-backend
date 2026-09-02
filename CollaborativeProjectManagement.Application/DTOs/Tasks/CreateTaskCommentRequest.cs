using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CreateTaskCommentRequest
    {
        [MaxLength(2000)]
        public required string Text { get; set; }
    }
}
