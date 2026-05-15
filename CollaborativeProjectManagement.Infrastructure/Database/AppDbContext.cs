using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionEntity>().HasData(
                Enum.GetValues<Permission>().Select(permission => new PermissionEntity
                {
                    Id = (int)permission,
                    Name = permission.ToString()
                })
            );

            // Table relationships
            modelBuilder.Entity<ProjectRole>().HasOne<Project>().WithMany(project => project.ProjectRoles).HasForeignKey(role => role.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>().HasOne(permission => permission.ProjectRole).WithMany().HasForeignKey(permission => permission.ProjectRoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>().HasOne<Project>().WithMany(project => project.ProjectMembers).HasForeignKey(member => member.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectMember>().HasOne(member => member.User).WithMany().HasForeignKey(member => member.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectRole>().HasMany(r => r.Permissions).WithMany().UsingEntity<RolePermission>();
            modelBuilder.Entity<ProjectMember>().HasOne(member => member.ProjectRole).WithMany().HasForeignKey(member => member.ProjectRoleId).OnDelete(DeleteBehavior.Restrict);

            // Table constraints
            modelBuilder.Entity<RolePermission>(builder =>
            {
                builder.HasIndex(rolePermission => new { rolePermission.ProjectRoleId, rolePermission.PermissionId }).IsUnique();
            });
            modelBuilder.Entity<ProjectMember>(builder =>
            {
                builder.HasIndex(projectMember => new { projectMember.ProjectId, projectMember.UserId }).IsUnique();
            });
        }
    }
}