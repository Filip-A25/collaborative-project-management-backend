using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public class RolePermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProjectRoleId { get; set; }
        public ProjectRole? ProjectRole { get; set; }

        public int PermissionId { get; set; }
        public PermissionEntity? Permission { get; set; }
    }
}
