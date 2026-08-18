using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Projects
{
    public class ProjectRolesRepository : IProjectRolesRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRolesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectRole> CreateProjectRoleAsync(ProjectRole projectRole)
        {
            _dbContext.ProjectRoles.Add(projectRole);
            await _dbContext.SaveChangesAsync();

            return projectRole;
        }

        public async Task<List<ProjectRole>> CreateProjectRolesAsync(List<ProjectRole> projectRoles)
        {
            _dbContext.ProjectRoles.AddRange(projectRoles);
            await _dbContext.SaveChangesAsync();

            return projectRoles;
        }

        public async Task AddRolePermissionsAsync(List<RolePermission> rolePermissionList)
        {
            await _dbContext.RolePermissions.AddRangeAsync(rolePermissionList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PermissionEntity>?> GetProjectMemberRolePermissionsAsync(Guid projectId, Guid userId)
        {
            ProjectRole? memberRole = await _dbContext.ProjectMembers
                .Where(member => member.ProjectId == projectId)
                .Where(member => member.UserId == userId)
                .Include(member => member.ProjectRole)
                .ThenInclude(role => role.Permissions)
                .Select(projectMember => projectMember.ProjectRole)
                .FirstOrDefaultAsync();

                if (memberRole == null || memberRole.Permissions == null || !memberRole.Permissions.Any()) return null;
            return memberRole.Permissions.ToList();
        }

        public async Task DeleteProjectRolesAsync(List<int> projectRoleIds, Guid projectId)
        {
            await _dbContext.ProjectRoles
                .Where(role => projectRoleIds.Contains(role.Id))
                .ExecuteDeleteAsync();
        }

        public async Task<ProjectRole?> GetProjectRoleAsync(Guid projectId, int projectRoleId)
        {
            return await _dbContext.ProjectRoles
                .Where(role => projectId == role.ProjectId)
                .FirstOrDefaultAsync(role => role.Id == projectRoleId);
        }

        public async Task<ProjectRole?> GetProjectRoleWithPermissionsAsync(Guid projectId, int projectRoleId)
        {
            return await _dbContext.ProjectRoles
                .Where(role => role.ProjectId == projectId)
                .Where(role => role.Id == projectRoleId)
                .Include(role => role.Permissions)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProjectRole>> GetAllRolesForProject(Guid projectId)
        {
            return await _dbContext.ProjectRoles.Where(role => role.ProjectId == projectId).Include(role => role.Permissions).ToListAsync();
        }

        public async Task UnassignPermissionsFromRole (int roleId, List<int> permissionIds)
        {
            List<RolePermission> rolePermissionsToRemove = await _dbContext.RolePermissions.Where(rolePermission => rolePermission.ProjectRoleId == roleId).Where(rolePermission => permissionIds.Contains(rolePermission.PermissionId)).ToListAsync();
            _dbContext.RolePermissions.RemoveRange(rolePermissionsToRemove);
        }

        public void AddRolePermissions (List<RolePermission> rolePermissions)
        {
            _dbContext.RolePermissions.AddRange(rolePermissions);
        }

        public async Task UpdateProjectRolesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
