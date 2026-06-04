using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public ProjectMemberDTO Commenter { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CommentDTO FromEntity(Comment comment) => new()
        {
            Id = comment.Id,
            Commenter = ProjectMemberDTO.FromEntity(comment.Commenter),
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };
    }
}
