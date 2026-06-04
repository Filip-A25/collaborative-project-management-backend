using CollaborativeProjectManagement.Application.Common;
using CollaborativeProjectManagement.Application.DTOs.Projects;
using CollaborativeProjectManagement.Application.Interfaces.Projects;
using CollaborativeProjectManagement.Domain.Entities.Auth;
using CollaborativeProjectManagement.Domain.Entities.Projects;
using CollaborativeProjectManagement.Domain.Interfaces.Auth;
using CollaborativeProjectManagement.Domain.Interfaces.Projects;

namespace CollaborativeProjectManagement.Application.Services.Projects
{
    public class ProjectInvitesService : IProjectInvitesService
    {
        private readonly IProjectInvitesRepository _projectInvitesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IProjectsRepository _projectsRepository;

        public ProjectInvitesService(IProjectInvitesRepository projectInvitesRepository, IUserRepository userRepository, IProjectAuthorizationService projectAuthorizationService, IProjectsRepository projectsRepository)
        {
            _projectInvitesRepository = projectInvitesRepository;
            _userRepository = userRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _projectsRepository = projectsRepository;
        }

        public async Task<ServiceResponse> CreateProjectInviteAsync(Guid userId, Guid projectId, CreateProjectInviteRequest request)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.ManageRoles);
            if (!userHasSufficientPermissions) return ServiceResponse.Forbidden(ResponseMessage.ProjectInvites.InvitesCreateError);

            User? invitedUser = await _userRepository.GetUserById(userId);

            if (invitedUser == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.Auth.UserNotFound);
            }

            ProjectInvite newProjectInvite = new ProjectInvite(projectId, request.InvitedUserId, request.RoleId, request.ExpiresAt);

            await _projectInvitesRepository.CreateProjectInviteAsync(newProjectInvite);

            return ServiceResponse.Ok(ResponseMessage.ProjectInvites.CreateSuccess);
        }

        public async Task<ServiceResponse> DeleteProjectInviteAsync(Guid userId, Guid projectId, int inviteId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.InviteMembers);
            if (!userHasSufficientPermissions) return ServiceResponse.Forbidden(ResponseMessage.ProjectInvites.InvitesDeleteError);

            await _projectInvitesRepository.DeleteProjectInviteAsync(projectId, inviteId);

            return ServiceResponse.Ok(ResponseMessage.ProjectInvites.DeleteSuccess);
        }

        public async Task<ServiceResponse> AcceptProjectInviteAsync(Guid userId, Guid projectId, int inviteId)
        {
            ProjectInvite? invite = await _projectInvitesRepository.GetProjectInviteAsync(projectId, inviteId);
            if (invite?.InvitedUserId != userId)
            {
                return ServiceResponse.InternalServerError(ResponseMessage.ProjectInvites.InternalUpdateError);
            }

            bool hasInviteExpired = DateTime.UtcNow > invite.ExpiresAt;
            if (hasInviteExpired)
            {
                return ServiceResponse.Gone(ResponseMessage.ProjectInvites.Expired);
            }

            User? invitedUser = await _userRepository.GetUserById(invite.InvitedUserId);
            if (invitedUser == null)
            {
                return ServiceResponse.NotFound(ResponseMessage.Auth.UserNotFound);
            }

            ProjectMember newProjectMember = new ProjectMember(invitedUser.Id, projectId, invite.InvitedUserRoleId);
            await _projectsRepository.AddMemberToProjectAsync(newProjectMember);
            await _projectInvitesRepository.UpdateProjectInviteToAcceptedAsync(invite);

            return ServiceResponse.Ok(ResponseMessage.ProjectInvites.AcceptSuccess);
        }

        public async Task<ServiceResponse<List<ProjectInvite>?>> GetAllUserInvitesAsync(Guid userId)
        {
            List<ProjectInvite>? invites = await _projectInvitesRepository.GetAllUserInvitesAsync(userId);
            return ServiceResponse<List<ProjectInvite>?>.Ok(invites, null);
        }

        public async Task<ServiceResponse<List<ProjectInvite>?>> GetAllProjectsInvitesAsync(Guid userId, Guid projectId)
        {
            bool userHasSufficientPermissions = await _projectAuthorizationService.CheckIfUserHasSufficientPermissionsAsync(projectId, userId, Permission.InviteMembers);
            if (!userHasSufficientPermissions) return ServiceResponse<List<ProjectInvite>?>.Forbidden(null, ResponseMessage.ProjectInvites.InvitesFetchError);

            List<ProjectInvite>? projectInvites = await _projectInvitesRepository.GetAllProjectsInvitesAsync(projectId);
            return ServiceResponse<List<ProjectInvite>?>.Ok(projectInvites, null);
        }
    }
}
