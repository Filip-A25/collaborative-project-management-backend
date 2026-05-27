using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid CommenterId { get; set; }
        public Guid ProjectTaskId { get; set; }
        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
