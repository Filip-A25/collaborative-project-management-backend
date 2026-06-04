using System.Data;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ProjectInvite> ProjectInvites { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionEntity>().HasData(
                Enum.GetValues<Permission>().Select(permission => new PermissionEntity
                {
                    Id = (int)permission,
                    Name = permission.ToString()
                })
            );


            // Project role and permissions relationships
            modelBuilder.Entity<ProjectRole>().HasOne<Project>().WithMany(project => project.ProjectRoles).HasForeignKey(role => role.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>().HasOne(permission => permission.ProjectRole).WithMany().HasForeignKey(permission => permission.ProjectRoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectRole>().HasMany(r => r.Permissions).WithMany().UsingEntity<RolePermission>();

            // Project member relationships
            modelBuilder.Entity<ProjectMember>().HasOne<Project>().WithMany(project => project.ProjectMembers).HasForeignKey(member => member.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>().HasOne(member => member.User).WithMany().HasForeignKey(member => member.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>().HasOne(member => member.ProjectRole).WithMany().HasForeignKey(member => member.ProjectRoleId).OnDelete(DeleteBehavior.Restrict);

            // Project invite relationships
            modelBuilder.Entity<ProjectInvite>().HasOne<Project>().WithMany().HasForeignKey(invite => invite.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectInvite>().HasOne<User>().WithMany().HasForeignKey(invite => invite.InvitedUserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectInvite>().HasOne<ProjectRole>().WithMany().HasForeignKey(invite => invite.InvitedUserRoleId).OnDelete(DeleteBehavior.Restrict);

            // Tasks relationships
            modelBuilder.Entity<ProjectTask>().HasOne(task => task.Creator).WithMany().HasForeignKey(task => task.CreatorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProjectTask>().HasOne(task => task.AssignedUser).WithMany().HasForeignKey(task => task.AssignedTo).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaskType>().HasOne<Project>().WithMany().HasForeignKey(type => type.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().HasOne(comment => comment.Commenter).WithMany().HasForeignKey(comment => comment.CommenterId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne<ProjectTask>().WithMany().HasForeignKey(comment => comment.ProjectTaskId).OnDelete(DeleteBehavior.Restrict);

            // Table constraints
            modelBuilder.Entity<RolePermission>(builder =>
            {
                builder.HasIndex(rolePermission => new { rolePermission.ProjectRoleId, rolePermission.PermissionId }).IsUnique();
            });
            modelBuilder.Entity<ProjectMember>(builder =>
            {
                builder.HasIndex(projectMember => new { projectMember.ProjectId, projectMember.UserId }).IsUnique();
            });
            modelBuilder.Entity<ProjectInvite>(builder =>
            {
                builder.HasIndex(invite => new { invite.ProjectId, invite.InvitedUserId }).IsUnique();
            });
        }
    }
}
