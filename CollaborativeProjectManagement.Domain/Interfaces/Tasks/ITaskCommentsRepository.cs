using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Domain.Interfaces.Tasks
{
    public interface ITaskCommentsRepository
    {
        Task<Comment?> CreateTaskCommentAsync(Comment comment);
        Task DeleteTaskCommentAsync(Guid taskId, int commentId);
        Task<Comment?> GetTaskCommentWithCommenterAsync(Guid taskId, int commentId);
        Task UpdateTaskCommentAsync();
        Task<List<Comment>?> GetAllTaskCommentsAsync(Guid taskId);
    }
}
