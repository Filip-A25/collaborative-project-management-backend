using CollaborativeProjectManagement.Domain.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories
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

        public async Task<List<ProjectTask>?> GetAllProjectTasks(Guid projectId)
        {
            return await _dbContext.ProjectTasks.Where(task => task.ProjectId == projectId).ToListAsync();
        }
    }
}
