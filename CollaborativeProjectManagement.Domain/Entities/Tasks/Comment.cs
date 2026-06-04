using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CollaborativeProjectManagement.Domain.Entities.Projects;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CommenterId { get; set; }
        public ProjectMember Commenter { get; set; }
        [Required]
        public Guid ProjectTaskId { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Comment(int commenterId, Guid taskId, string text)
        {
            CommenterId = commenterId;
            ProjectTaskId = taskId;
            Text = text;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private Comment() { }
    }
}
