using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Domain.Entities.Tasks
{
    public class TaskType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        public required string Title { get; set; }
    }
}
