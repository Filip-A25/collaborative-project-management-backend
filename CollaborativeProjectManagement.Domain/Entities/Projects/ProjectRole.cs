using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public class ProjectRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Guid ProjectId { get; set; }
        [MaxLength(60)]
        public required string Name { get; set; }
        public required string Color { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PermissionEntity>? Permissions { get; set; }
    }
}
