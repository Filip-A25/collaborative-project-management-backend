using System;
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
    public class TaskTypesService : ITaskTypesService
    {
        private readonly ITaskTypesRepository _taskTypesRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IProjectsRepository _projectsRepository;

        public TaskTypesService(ITaskTypesRepository taskTypesRepository, IProjectAuthorizationService projectAuthorizationService, IProjectsRepository projectsRepository)
        {
            _taskTypesRepository = taskTypesRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _projectsRepository = projectsRepository;
        }

        public async Task<ServiceResponse<TaskType?>> CreateTaskTypeAsync(Guid userId, Guid projectId, CreateTaskTypeRequest request)
        {
            Project? targetProject = await _projectsRepository.GetProjectAsync(projectId);
            if (targetProject == null)
            {
                return ServiceResponse<TaskType?>.NotFound(null, ResponseMessage.Projects.ProjectNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions) return ServiceResponse<TaskType?>.Forbidden(null, ResponseMessage.Projects.ProjectManageError);

            TaskType newType = new TaskType
            {
                ProjectId = projectId,
                Title = request.Title
            };

            await _taskTypesRepository.CreateTaskTypeAsync(newType);

            return ServiceResponse<TaskType?>.Ok(newType, ResponseMessage.TaskTypes.CreateSuccess);
        }

        public async Task<ServiceResponse> DeleteTaskTypeAsync(Guid userId, DeleteTaskTypeRequest request)
        {
            bool doesTaskTypeExist = await CheckIfTaskTypeExists(request.ProjectId, request.TypeId);
            if (!doesTaskTypeExist)
            {
                return ServiceResponse<TaskType?>.NotFound(null, ResponseMessage.TaskTypes.TaskTypeNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(request.ProjectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions) return ServiceResponse<TaskType?>.Forbidden(null, ResponseMessage.Projects.ProjectManageError);

            TaskType? type = await _taskTypesRepository.GetTaskTypeByIdAsync(request.ProjectId, request.TypeId);

            if (type == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.TaskTypes.TaskTypeNotFound);
            }

            await _taskTypesRepository.DeleteTaskTypeAsync(request.ProjectId, request.TypeId);

            return ServiceResponse.NoContent(ResponseMessage.TaskTypes.DeleteSuccess);
        }

        public async Task<ServiceResponse<TaskType?>> ChangeTaskTypeTitleAsync(Guid userId, Guid projectId, int typeId, ChangeTaskTypeTitleRequest request)
        {
            bool doesTaskTypeExist = await CheckIfTaskTypeExists(projectId, typeId);
            if (!doesTaskTypeExist)
            {
                return ServiceResponse<TaskType?>.NotFound(null, ResponseMessage.TaskTypes.TaskTypeNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions) return ServiceResponse<TaskType?>.Forbidden(null, ResponseMessage.Projects.ProjectManageError);

            TaskType? type = await _taskTypesRepository.ChangeTaskTypeTitleAsync(projectId, typeId, request.Title);
            if (type == null)
            {
                return ServiceResponse<TaskType?>.NotFound(null, ResponseMessage.Tasks.TaskNotFound);
            }

            return ServiceResponse<TaskType?>.Ok(type, ResponseMessage.TaskTypes.UpdateSuccess);
        }

        public async Task<ServiceResponse<TaskType?>> GetTaskTypeByIdAsync(Guid userId, Guid projectId, int typeId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions) return ServiceResponse<TaskType?>.Forbidden(null, ResponseMessage.Projects.ProjectManageError);

            TaskType? type = await _taskTypesRepository.GetTaskTypeByIdAsync(projectId, typeId);

            if (type == null)
            {
                return ServiceResponse<TaskType?>.NotFound(null, ResponseMessage.TaskTypes.TaskTypeNotFound);
            }

            return ServiceResponse<TaskType?>.Ok(type, null);
        }

        private async Task<bool> CheckIfTaskTypeExists(Guid projectId, int typeId)
        {
            TaskType? type = await _taskTypesRepository.GetTaskTypeByIdAsync(projectId, typeId);
            return type != null ? true : false;
        }
    }
}
