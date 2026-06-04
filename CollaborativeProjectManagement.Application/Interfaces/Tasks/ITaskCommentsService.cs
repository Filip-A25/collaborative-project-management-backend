using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;

namespace CollaborativeProjectManagement.Application.Interfaces.Tasks
{
    public interface ITaskCommentsService
    {
        Task<ServiceResponse<CommentDTO?>> CreateTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, CreateTaskCommentRequest request);
        Task<ServiceResponse> DeleteTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, int commentId);
        Task<ServiceResponse<CommentDTO?>> UpdateTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, int commentId, UpdateTaskCommentRequest request);
        Task<ServiceResponse<List<CommentDTO>?>> GetAllTaskCommentsAsync(Guid userId, Guid projectId, Guid taskId);
    }
}
