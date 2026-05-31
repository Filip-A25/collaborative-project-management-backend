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

        public async Task<ServiceResponse<ProjectTaskDTO?>> CreateTaskAsync(Guid userId, CreateProjectTaskRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectTaskDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            List<ProjectMember>? allProjectMembers = await _projectsRepository.GetAllProjectMembers(request.ProjectId);
            bool projectHasAssignedMember = allProjectMembers != null ? allProjectMembers.Any(member => member.UserId == request.AssignedTo) : false;

            if (!projectHasAssignedMember)
            {
                return ServiceResponse<ProjectTaskDTO?>.NotFound(null, ResponseMessage.Tasks.MemberNotFound);
            }

            ProjectTask newTask = new ProjectTask(request.ProjectId, request.Title, request.Description, request.CreatorId, request.AssignedTo, request.Priority, request.Status, request.Type, request.StartDate, request.DueDate);
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

        public async Task<ServiceResponse<List<ProjectTaskDTO>?>> GetAllProjectTasksAsync(Guid userId, Guid projectId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ViewProject);
            if (!userHasSufficientPermissions) return ServiceResponse<List<ProjectTaskDTO>?>.Forbidden(null, ResponseMessage.Tasks.ViewTasksError);

            List<ProjectTask>? allTasks = await _tasksRepository.GetAllProjectTasks(projectId);
            if (allTasks == null || !allTasks.Any())
            {
                return ServiceResponse<List<ProjectTaskDTO>?>.NotFound(null, ResponseMessage.Tasks.ProjectTasksNotFound);
            }

            List<ProjectTaskDTO> allTaskDtos = allTasks.Select(ProjectTaskDTO.FromEntity).ToList();
            return ServiceResponse<List<ProjectTaskDTO>?>.Ok(allTaskDtos, null);
        }

        public async Task<ServiceResponse<ProjectTaskDTO?>> GetTaskByIdAsync(Guid userId, Guid projectId, Guid taskId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ViewProject);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectTaskDTO?>.Forbidden(null, ResponseMessage.Tasks.ViewTasksError);

            ProjectTask? task = await _tasksRepository.GetTaskWithUsersAsync(projectId, taskId);
            if (task == null)
            {
                return ServiceResponse<ProjectTaskDTO?>.NotFound(null, ResponseMessage.Tasks.TaskNotFound);
            }

            ProjectTaskDTO taskDto = ProjectTaskDTO.FromEntity(task);
            return ServiceResponse<ProjectTaskDTO?>.Ok(taskDto, null);
        }

        public async Task<ServiceResponse<ProjectTaskDTO?>> UpdateTaskAsync(Guid userId, Guid projectId, Guid taskId, UpdateTaskRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageTasks);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectTaskDTO?>.Forbidden(null, ResponseMessage.Tasks.TasksManageError);

            ProjectTask? targetTask = await _tasksRepository.GetTaskWithUsersAsync(projectId, taskId);
            if (targetTask == null)
            {
                return ServiceResponse<ProjectTaskDTO?>.NotFound(null, ResponseMessage.Tasks.TaskNotFound);
            }

            targetTask.Title = request.Title ?? targetTask.Title;
            targetTask.Description = request.Description ?? targetTask.Description;
            targetTask.AssignedTo = request.AssignedTo ?? targetTask.AssignedTo;
            targetTask.Priority = request.Priority ?? targetTask.Priority;
            targetTask.Status = request.Status ?? targetTask.Status;
            targetTask.TypeId = request.TypeId ?? targetTask.TypeId;
            targetTask.StartDate = request.StartDate ?? targetTask.StartDate;
            targetTask.DueDate = request.DueDate ?? targetTask.DueDate;

            await _tasksRepository.UpdateTaskAsync();

            ProjectTaskDTO taskDto = ProjectTaskDTO.FromEntity(targetTask);
            return ServiceResponse<ProjectTaskDTO?>.Ok(taskDto, ResponseMessage.Tasks.UpdateSuccess);
        }
    }
}
