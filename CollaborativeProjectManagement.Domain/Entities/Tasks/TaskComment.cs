using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class TaskComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid ProjectTaskId { get; set; }
        public ProjectTask? Task { get; set; }
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
