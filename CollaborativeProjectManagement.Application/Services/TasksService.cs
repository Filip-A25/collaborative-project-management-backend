using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Tasks;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Tasks;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IProjectsRepository _projectsRepository;

        public TasksService(ITasksRepository tasksRepository, IProjectAuthorizationService projectAuthorizationService, IProjectsRepository projectsRepository)
        {
            _tasksRepository = tasksRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _projectsRepository = projectsRepository;
        }

        public async Task<ServiceResponse<ProjectTaskDTO?>> CreateTaskAsync(Guid userId, Guid projectId, CreateProjectTaskRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectTaskDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            List<ProjectMember>? allProjectMembers = await _projectsRepository.GetAllProjectMembers(projectId);
            bool projectHasAssignedMember = allProjectMembers != null ? allProjectMembers.Any(member => member.UserId == request.AssignedTo) : false;

            if (!projectHasAssignedMember)
            {
                return ServiceResponse<ProjectTaskDTO?>.NotFound(null, ResponseMessage.Tasks.MemberNotFound);
            }

            ProjectTask newTask = new ProjectTask(projectId, request.Title, request.Description, request.CreatorId, request.AssignedTo, request.Priority, request.Status, request.Type, request.StartDate, request.DueDate);
            await _tasksRepository.CreateTaskAsync(newTask);

            ProjectTaskDTO taskDto = ProjectTaskDTO.FromEntity(newTask);

            return ServiceResponse<ProjectTaskDTO?>.Ok(taskDto, ResponseMessage.Tasks.CreateSuccess);
        }

        public async Task<ServiceResponse> DeleteTaskAsync(Guid userId, DeleteTaskRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse.Forbidden(ResponseMessage.Tasks.TasksManageError);

            await _tasksRepository.DeleteTaskAsync(request.ProjectId, request.TaskId);
            return ServiceResponse.NoContent(ResponseMessage.Tasks.DeleteSuccess);
        }

        public async Task<ServiceResponse<List<ProjectTaskDTO>?>> GetAllProjectTasks(Guid userId, Guid projectId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<List<ProjectTaskDTO>?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            List<ProjectTask>? allTasks = await _tasksRepository.GetAllProjectTasks(projectId);
            if (allTasks == null || !allTasks.Any())
            {
                return ServiceResponse<List<ProjectTaskDTO>?>.NotFound(null, ResponseMessage.Tasks.ProjectTasksNotFound);
            }

            List<ProjectTaskDTO> allTaskDtos = allTasks.Select(ProjectTaskDTO.FromEntity).ToList();
            return ServiceResponse<List<ProjectTaskDTO>?>.Ok(allTaskDtos, null);
        }
    }
}
