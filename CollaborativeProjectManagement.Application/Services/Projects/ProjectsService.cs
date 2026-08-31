using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Tasks;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services.Projects
{   
        public class ProjectsService : IProjectsService
        {
        private readonly IProjectsRepository _projectsRepository;   
        private readonly IUserRepository _userRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService; 
        private readonly IProjectRolesRepository _projectRolesRepository;       
        private readonly IProjectRolesService _projectRolesService;
        private readonly ITasksService _tasksService;

            public ProjectsService(IProjectsRepository projectsRepository, IUserRepository userRepository, IProjectAuthorizationService projectAuthorizationService, IProjectRolesRepository projectRolesRepository, IProjectRolesService projectRolesService, ITasksService tasksService)
            {
                _projectsRepository = projectsRepository;
                _userRepository = userRepository;
                _projectAuthorizationService = projectAuthorizationService;
                _projectRolesRepository = projectRolesRepository;
                _projectRolesService = projectRolesService;
                _tasksService = tasksService;
            }

            public async Task<ServiceResponse<ProjectDTO?>> CreateProjectAsync(Guid userId, CreateProjectRequest request)
        {
            var newProject = new Project(request.Name, userId, request.Description, ProjectStatus.Planning, request.StartDate, request.EndDate, request.Currency, request.BudgetAmount);

            await _projectsRepository.CreateProjectAsync(newProject);
            await _projectRolesService.AssignCreatorRoleToUser(newProject.Id, userId);

            newProject = await _projectsRepository.GetProjectWithMembersAsync(newProject.Id);

            ProjectDTO newProjectDto = ProjectDTO.FromEntity(newProject);

            if (request.Roles == null || !request.Roles.Any())
            {
                return ServiceResponse<ProjectDTO?>.Created(newProjectDto, null);
            }

            List<ProjectRole> newProjectRoles = request.Roles.Select(role => new ProjectRole
            {
                ProjectId = newProject.Id,
                Name = role.Name,
                Color = role.Color
            }).ToList();

            await _projectRolesRepository.CreateProjectRolesAsync(newProjectRoles);

            return ServiceResponse<ProjectDTO?>.Created(newProjectDto, ResponseMessage.Projects.CreateSuccess);
        }

        public async Task<ServiceResponse> DeleteProjectAsync(Guid projectId, Guid userId)
        {
            Project? project = await _projectsRepository.GetProjectWithMembersAsync(projectId);

            if (project == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.Projects.ProjectNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions)
            {
                return ServiceResponse.Forbidden(ResponseMessage.Projects.ProjectRoleDeleteError);
            }

            await _tasksService.DeleteAllProjectTasksAsync(projectId);
            await _projectsRepository.DeleteProjectAsync(projectId);

            return ServiceResponse.NoContent(ResponseMessage.Projects.DeleteSuccess);
        }

        public async Task<ServiceResponse<ProjectDTO?>> GetProjectAsync(Guid projectId, Guid userId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ViewProject);
            if (!userHasSufficientPermissions)
            {
                return ServiceResponse<ProjectDTO?>.Forbidden(null, ResponseMessage.Projects.ProjectRoleViewError);
            }

            Project? project = await _projectsRepository.GetProjectWithMembersAsync(projectId);

            if (project == null)
            {
                return ServiceResponse<ProjectDTO?>.NotFound(null, ResponseMessage.Projects.ProjectNotFound);
            }

            ProjectDTO projectDto = ProjectDTO.FromEntity(project);   
            return ServiceResponse<ProjectDTO?>.Ok(projectDto, null);
        }

        public async Task<ServiceResponse<List<ProjectDTO>?>> GetAllProjectsForUserAsync(Guid userId)
        {
            List<Guid> projectIds = await _projectsRepository.GetAllProjectIdsForUserAsync(userId);

            List<Project> projects = await _projectsRepository.GetAllProjectsForUserAsync(projectIds);
            List<ProjectDTO> projectDtos = projects.Select(ProjectDTO.FromEntity).ToList();

            return ServiceResponse<List<ProjectDTO>?>.Ok(projectDtos, null);
        }

        public async Task<ServiceResponse> RemoveMemberFromProjectAsync(Guid userId, Guid projectId, int memberId)
        {
            ProjectMember? targetMember = await _projectsRepository.GetProjectMemberByIdAsync(projectId, memberId);
            bool isMemberRemovingHimself = userId == targetMember?.User.Id;

            if (!isMemberRemovingHimself)
            {
                bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.RemoveMembers);   
                if (!userHasSufficientPermissions)
                {
                    return ServiceResponse.Forbidden(ResponseMessage.Projects.ProjectMembersRemoveError);
                }
            }

            if (targetMember == null)
            {
                ServiceResponse.NotFound(ResponseMessage.Projects.MemberNotFound);
            }

            if (targetMember.ProjectRole.IsCreatorRole)
            {
                ServiceResponse.Forbidden(ResponseMessage.Projects.CreatorRemoveFail);
            }

            await _tasksService.RemoveCreatorFromTasksAsync(projectId, memberId);
            await _projectsRepository.RemoveMemberFromProjectAsync(projectId, memberId);

            return ServiceResponse.NoContent(ResponseMessage.Projects.MemberRemoveSuccess);
        }

        public async Task<ServiceResponse<ProjectDTO?>> UpdateProjectAsync(Guid userId, Guid projectId, UpdateProjectRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions) return ServiceResponse<ProjectDTO?>.Forbidden(null, ResponseMessage.Projects.ProjectManageError);
        
        
            Project? targetProject = await _projectsRepository.GetProjectWithFullMembersAsync(projectId);
            if (targetProject == null)
            {
                return ServiceResponse<ProjectDTO?>.NotFound(null, ResponseMessage.Projects.ProjectNotFound);
            }

            ProjectStatus parsedStatus = Enum.Parse<ProjectStatus>(request.Status);

            targetProject.Name = request.Name ?? targetProject.Name;
            targetProject.Description = request.Description ?? targetProject.Description;
            targetProject.StartDate = request.StartDate ?? targetProject.StartDate;
            targetProject.EndDate = request.EndDate ?? targetProject.EndDate;
            targetProject.Currency = request.Currency ?? targetProject.Currency;
            targetProject.BudgetAmount = request.BudgetAmount ?? targetProject.BudgetAmount;
            targetProject.Status = parsedStatus;
            targetProject.CompletedDate = request.CompletedDate ?? targetProject.CompletedDate;

            if (request.Roles != null && request.Roles.Count > 0)
            {
                await _projectRolesService.BulkUpdateProjectRoles(projectId, request.Roles);                
            }

            await _projectsRepository.UpdateProjectAsync();

            ProjectDTO projectDto = ProjectDTO.FromEntity(targetProject);
            return ServiceResponse<ProjectDTO?>.Ok(projectDto, ResponseMessage.Projects.UpdateSuccess);
        }
    };
}
