using System.ComponentModel.DataAnnotations;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public required string Name { get; set; }
    }
}
