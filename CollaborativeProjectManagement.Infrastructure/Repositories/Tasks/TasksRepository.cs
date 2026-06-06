using CollaborativeProjectManagement.Domain.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Tasks
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _dbContext;

        public TasksRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            _dbContext.ProjectTasks.Add(task);
            await _dbContext.SaveChangesAsync();

            ProjectTask? createdTask = await _dbContext.ProjectTasks.Include(task => task.Creator).Include(task => task.AssignedUser).FirstOrDefaultAsync(createdTask => createdTask.Id == task.Id);

            return createdTask ?? task;
        }

        public async Task DeleteTaskAsync(Guid projectId, Guid taskId)
        {
            await _dbContext.ProjectTasks.Where(task => task.ProjectId == projectId).Where(task => task.Id == taskId).ExecuteDeleteAsync();
        }

        public async Task<List<ProjectTask>> GetAllProjectTasksAsync(Guid projectId)
        {
            return await _dbContext.ProjectTasks.Where(task => task.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectTask?> GetTaskWithUsersAsync(Guid projectId, Guid taskId)
        {
            return await _dbContext.ProjectTasks.Include(task => task.Creator).Include(task => task.AssignedUser).FirstOrDefaultAsync(task => task.Id == taskId);
        }

        public async Task<ProjectTask?> GetTaskAsync(Guid projectId, Guid taskId)
        {
            return await _dbContext.ProjectTasks.FirstOrDefaultAsync(task => task.Id == taskId);
        }

        public async Task UpdateTaskAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProjectTask>> GetAllTasksFromMemberAsync(Guid projectId, int memberId)
        {
            return await _dbContext.ProjectTasks.Where(task => task.ProjectId == projectId).Where(task => task.CreatorId == memberId).ToListAsync();
        }

        public async Task DeleteAllProjectTasksAsync(Guid projectId)
        {
            List<ProjectTask> allTasks = await GetAllProjectTasksAsync(projectId);

            _dbContext.ProjectTasks.RemoveRange(allTasks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
