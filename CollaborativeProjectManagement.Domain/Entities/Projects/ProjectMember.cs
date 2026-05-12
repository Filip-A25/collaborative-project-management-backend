using System.ComponentModel.DataAnnotations.Schema;
using CollaborativeProjectManagement.Domain.Entities.Auth;

namespace CollaborativeProjectManagement.Domain.Entities.Projects
{
    public class ProjectMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public int ProjectRoleId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; private set; }
        public ProjectRole? ProjectRole { get; private set; }

        public ProjectMember(Guid userId, Guid projectId, int projectRoleId) 
        {
            UserId = userId;
            ProjectId = projectId;
            ProjectRoleId = projectRoleId;
            JoinedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private ProjectMember() { }
    }
}