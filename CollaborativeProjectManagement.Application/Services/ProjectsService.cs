using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IProjectRolesRepository _projectRolesRepository;
        private readonly IProjectRolesService _projectRolesService;

        public ProjectsService(IProjectsRepository projectsRepository, IUserRepository userRepository, IProjectAuthorizationService projectAuthorizationService, IProjectRolesRepository projectRolesRepository, IProjectRolesService projectRolesService)
        {
            _projectsRepository = projectsRepository;
            _userRepository = userRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _projectRolesRepository = projectRolesRepository;
            _projectRolesService = projectRolesService;
        }

        public async Task<ServiceResponse<ProjectDTO?>> CreateProjectAsync(Guid userId, CreateProjectRequest request)
        {
            UserRole userRole = await _userRepository.GetUserRoleId(userId);

            if (userRole != UserRole.Admin)
            {
                return ServiceResponse<ProjectDTO?>.Forbidden(null, ResponseMessage.Projects.AuthorizationError);
            }

            var newProject = new Project(request.Name, userId, request.Description, ProjectStatus.Planning, request.StartDate, request.EndDate, request.Currency, request.BudgetAmount);

            await _projectsRepository.CreateProjectAsync(newProject);
            await AssignCreatorRoleToUser(newProject.Id, userId);

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

            return ServiceResponse<ProjectDTO?>.Created(newProjectDto, null);
        }

        public async Task<ServiceResponse> DeleteProjectAsync(Guid projectId, Guid userId)
        {
            Project? project = await _projectsRepository.GetProjectAsync(projectId);

            if (project == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.Projects.ProjectNotFound);
            }

            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageProject);
            if (!userHasSufficientPermissions)
            {
                return ServiceResponse.Forbidden(ResponseMessage.Projects.ProjectRoleDeleteError);
            }

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

            Project? project = await _projectsRepository.GetProjectAsync(projectId);

            if (project == null)
            {
                return ServiceResponse<ProjectDTO?>.NotFound(null, ResponseMessage.Projects.ProjectNotFound);
            }

            ProjectDTO projectDto = ProjectDTO.FromEntity(project);
            return ServiceResponse<ProjectDTO?>.Ok(projectDto, null);
        }

        public async Task<ServiceResponse<List<ProjectDTO>?>> GetAllProjectsForUserAsync(Guid userId)
        {
            List<Guid>? projectIds = await _projectsRepository.GetAllProjectIdsForUserAsync(userId);
            if (projectIds == null || !projectIds.Any())
            {
                return ServiceResponse<List<ProjectDTO>?>.NotFound(null, ResponseMessage.Projects.ProjectsDontExist);
            }

            List<Project> projects = await _projectsRepository.GetAllProjectsForUserAsync(projectIds);
            List<ProjectDTO> projectDtos = projects.Select(ProjectDTO.FromEntity).ToList();

            return ServiceResponse<List<ProjectDTO>?>.Ok(projectDtos, null);
        }

        private async Task AssignCreatorRoleToUser(Guid projectId, Guid userId)
        {
            ProjectRole creatorRole = await _projectRolesService.AddCreatorRole(projectId, userId);
            ProjectMember newMember = new ProjectMember(userId, projectId, creatorRole.Id);

            await _projectsRepository.AddMemberToProjectAsync(newMember);
        }
    };
}
