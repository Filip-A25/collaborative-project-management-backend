using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using CollaborativeProjectManagement.Domain.Interfaces.Tasks;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Tasks
{
    public class TaskTypesRepository : ITaskTypesRepository
    {
        private readonly AppDbContext _dbContext;

        public TaskTypesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskType> CreateTaskTypeAsync(TaskType type)
        {
            _dbContext.TaskTypes.Add(type);
            await _dbContext.SaveChangesAsync();

            return type;
        }

        public async Task DeleteTaskTypeAsync(Guid projectId, int typeId)
        {
            await _dbContext.TaskTypes.Where(type => type.ProjectId == projectId).Where(type => type.Id == typeId).ExecuteDeleteAsync();
        }

        public async Task<TaskType?> ChangeTaskTypeTitleAsync(Guid projectId, int typeId, string title)
        {
            TaskType? type = await _dbContext.TaskTypes.Where(type => type.ProjectId == projectId).FirstOrDefaultAsync(type => type.Id == typeId);

            if (type == null) return null;

            type.Title = title;
            await _dbContext.SaveChangesAsync();

            return type;
        }

        public async Task<TaskType?> GetTaskTypeByIdAsync(Guid projectId, int typeId)
        {
            return await _dbContext.TaskTypes.Where(type => type.ProjectId == projectId).FirstOrDefaultAsync(type => type.Id == typeId);
        }
    }
}
