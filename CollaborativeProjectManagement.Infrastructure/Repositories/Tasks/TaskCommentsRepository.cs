using CollaborativeProjectManagement.Domain.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeProjectManagement.Infrastructure.Repositories.Tasks
{
    public class TaskCommentsRepository : ITaskCommentsRepository
    {
        private readonly AppDbContext _dbContext;

        public TaskCommentsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment?> CreateTaskCommentAsync(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            Comment? newComment = await _dbContext.Comments
                .Include(selectedComment => selectedComment.Commenter)
                    .ThenInclude(commenter => commenter.User)
                .FirstOrDefaultAsync(selectedComment => selectedComment.Id == comment.Id);

            return comment;
        }

        public async Task DeleteTaskCommentAsync(Guid taskId, int commentId)
        {
            await _dbContext.Comments
                .Where(comment => comment.ProjectTaskId == taskId)
                .Where(comment => comment.Id == commentId)
                .ExecuteDeleteAsync();
        }

        public async Task<Comment?> GetTaskCommentWithCommenterAsync(Guid taskId, int commentId)
        {
            return await _dbContext.Comments
                .Where(comment => comment.ProjectTaskId == taskId)
                .Include(comment => comment.Commenter)
                    .ThenInclude(commenter => commenter.User)
                .FirstOrDefaultAsync(comment => comment.Id == commentId);
        }

        public async Task UpdateTaskCommentAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Comment>?> GetAllTaskCommentsAsync(Guid taskId)
        {
            return await _dbContext.Comments
                .Where(comment => comment.ProjectTaskId == taskId)
                .Include(comment => comment.Commenter)
                    .ThenInclude(commenter => commenter.User)
                .ToListAsync();
        }
    }
}
