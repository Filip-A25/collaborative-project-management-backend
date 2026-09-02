using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string CommenterFirstName { get; set; }
        public string CommenterLastName { get; set; }
        public string CommenterUsername { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CommentDTO FromEntity(Comment comment) => new()
        {
            Id = comment.Id,
            CommenterFirstName = comment.Commenter.User.FirstName,
            CommenterLastName = comment.Commenter.User.LastName,
            CommenterUsername = comment.Commenter.User.Username,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };
    }
}
