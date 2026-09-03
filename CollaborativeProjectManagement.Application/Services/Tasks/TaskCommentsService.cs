using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Tasks;

namespace CollaborativeProjectManagement.Application.Services.Tasks
{
    public class TaskCommentsService : ITaskCommentsService
    {
        private readonly ITaskCommentsRepository _taskCommentsRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly ITasksRepository _tasksRepository;
        private readonly IProjectsRepository _projectsRepository;

        public TaskCommentsService(ITaskCommentsRepository taskCommentsRepository, IProjectAuthorizationService projectAuthorizationService, ITasksRepository tasksRepository, IProjectsRepository projectsRepository)
        {
            _taskCommentsRepository = taskCommentsRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _tasksRepository = tasksRepository;
            _projectsRepository = projectsRepository;
        }

        public async Task<ServiceResponse<CommentDTO?>> CreateTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, CreateTaskCommentRequest request)
        {
            ProjectMember? member = await _projectsRepository.GetProjectMemberAsync(projectId, userId);
            if (member == null)
            {
                return ServiceResponse<CommentDTO?>.NotFound(null, ResponseMessage.Projects.UserNotMember);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<CommentDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            Comment newComment = new Comment(member.Id, taskId, request.Text);
            Comment? createdComment = await _taskCommentsRepository.CreateTaskCommentAsync(newComment);

            CommentDTO createdCommentDto = CommentDTO.FromEntity(createdComment);

            return ServiceResponse<CommentDTO?>.Created(createdCommentDto, ResponseMessage.TaskComments.CreateSuccess);
        }

        public async Task<ServiceResponse> DeleteTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, int commentId)
        {
            Comment? targetComment = await _taskCommentsRepository.GetTaskCommentWithCommenterAsync(taskId, commentId);
            if (targetComment == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.Tasks.TaskNotFound);
            }

            if (targetComment.Commenter.User.Id != userId)
            {
                return ServiceResponse.Forbidden(ResponseMessage.TaskComments.NoPermissionDelete);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<CommentDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            await _taskCommentsRepository.DeleteTaskCommentAsync(taskId, commentId);
            return ServiceResponse.NoContent(ResponseMessage.TaskComments.DeleteSuccess);
        }

        public async Task<ServiceResponse<CommentDTO?>> UpdateTaskCommentAsync(Guid userId, Guid projectId, Guid taskId, int commentId, UpdateTaskCommentRequest request)
        {
            Comment? targetComment = await _taskCommentsRepository.GetTaskCommentWithCommenterAsync(taskId, commentId);
            if (targetComment == null)
            {
                return ServiceResponse<CommentDTO?>.NotFound(null, ResponseMessage.Tasks.TaskNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<CommentDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            int? memberId = await GetProjectMemberIdAsync(projectId, userId);
            if (memberId == null)
            {
                return ServiceResponse<CommentDTO?>.NotFound(null, "");
            }

            if (targetComment.CommenterId != memberId)
            {
                return ServiceResponse<CommentDTO?>.Forbidden(null, ResponseMessage.TaskComments.NoPermission);
            }

            targetComment.Text = request.Text;
            await _taskCommentsRepository.UpdateTaskCommentAsync();

            CommentDTO commentDto = CommentDTO.FromEntity(targetComment);
            return ServiceResponse<CommentDTO?>.Ok(null, ResponseMessage.TaskComments.UpdateSuccess);
        }

        public async Task<ServiceResponse<List<CommentDTO>?>> GetAllTaskCommentsAsync(Guid userId, Guid projectId, Guid taskId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ViewProject);
            if (!userHasSufficientPermissions) return ServiceResponse<List<CommentDTO>?>.Forbidden(null, ResponseMessage.TaskComments.TaskCommentsViewError);

            List<Comment>? allTaskComments = await _taskCommentsRepository.GetAllTaskCommentsAsync(taskId);

            if (allTaskComments == null || !allTaskComments.Any())
            {
                return ServiceResponse<List<CommentDTO>?>.NotFound(null, ResponseMessage.TaskComments.BatchNotFound);
            }

            List<CommentDTO> allTaskCommentDtos = allTaskComments.Select(comment => CommentDTO.FromEntity(comment)).ToList();
            return ServiceResponse<List<CommentDTO>?>.Ok(allTaskCommentDtos, null);
        }

        private async Task<int?> GetProjectMemberIdAsync(Guid projectId, Guid userId)
        {
            ProjectMember? member = await _projectsRepository.GetProjectMemberAsync(projectId, userId);
            if (member == null) return null;

            return member.Id;
        }
    }
}
