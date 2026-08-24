namespace CollaborativeProjectManagement.Application.Common
{
    public static class ResponseMessage
    {
        public static class Common
        {
            public const string InternalError = "Unexpected error occured. Please try again.";
        }

        public static class Auth
        {
            // Error messages
            public const string RegisterConflict = "An account with this email already exists.";
            public const string UserNotFound = "User could not be found.";
            public const string IncorrectPassword = "Incorrect password entered.";
            public const string NonUniqueUsername = "This username is already taken.";
            public const string NonUniqueEmail = "This email is already taken.";

            // Success messages
            public const string RegisterSuccess = "User has been successfully registered.";
            public const string LoginSuccess = "User has successfully logged in.";
            public const string UpdateSuccess = "User has been successfully updated.";
        }

        public static class Projects
        {
            // Error messages
            public const string AuthorizationError = "User has to be an Admin to create a project.";
            public const string ProjectNotFound = "Project could not be found.";
            public const string ProjectsDontExist = "User does not have any projects.";
            public const string UserNotMember = "User is not a member of this project.";
            public const string MemberNotFound = "Member could not be found.";
            public const string CreatorRemoveFail = "Creator cannot be removed from the project.";
    
            // Unauthorized role errors
            public const string ProjectRoleDeleteError = "User does not have sufficient permissions to delete the project.";
            public const string ProjectRoleViewError = "User does not have sufficient permissions to view the project.";
            public const string ProjectManageError = "User does not have sufficient permissions to manage this project.";
            public const string ProjectMembersRemoveError = "User does not have sufficient permissions to remove members from this project.";

            // Success messages
            public const string CreateSuccess = "Project has been successfully created.";
            public const string DeleteSuccess = "Project has been successfully deleted.";
            public const string MemberRemoveSuccess = "Member has been successfully removed from the project.";
            public const string UpdateSuccess = "Project has been successfully updated.";
        }

        public static class ProjectRoles
        {
            // Error messages
            public const string RoleNotFound = "Role could not be found.";
            public const string ProjectIdMissing = "Project ID needs to be provided to create a role.";
            // Unauthorized role errors
            public const string RolesManageError = "User does not have sufficient permissions to manage project roles.";

            // Success messages
            public const string AddPermissionsSuccess = "Successfully added permissions to the role.";
            public const string DeleteBatchSuccess = "Project roles have been successfully deleted.";
        }

        public static class ProjectInvites
        {
            // Error messages
            public const string UsersNotFound = "Users could not be found.";
            public const string Expired = "Unable to update the invite. Invite has expired.";
            public const string InviteNotFound = "Invite not found.";

            // Unauthorized invites errors
            public const string InvitesCreateError = "User does not have sufficient permissions to create invites.";
            public const string InvitesDeleteError = "User does not have sufficient permissions to delete invites.";
            public const string InvitesFetchError = "User does not have sufficient permissions to fetch project invites.";
            public const string InvitesUserAlreadyMemberError = "User is already a member of the project.";

            // Success messages
            public const string CreateSuccess = "User has been invited to a project.";
            public const string DeleteSuccess = "Invite has been successfully deleted.";
            public const string AcceptSuccess = "Invite has been successfully accepted. You are now a member of the project.";
        }

        public static class Tasks
        {
            // Error messages
            public const string TasksManageError = "User does not have sufficient permissions to manage tasks.";
            public const string ProjectTasksNotFound = "No tasks were found for the project.";
            public const string MemberNotInProject = "Assigned user is not a member in this project.";
            public const string TaskNotFound = "Task not found.";

            // Unauthorized tasks errros
            public const string ViewTasksError = "User does not have sufficient permissions to view tasks in this project.";

            // Success messages
            public const string CreateSuccess = "Task has been successfully created.";
            public const string DeleteSuccess = "Task has been successfully deleted.";
            public const string UpdateSuccess = "Task has been successfully updated.";
        }

        public static class TaskTypes
        {
            // Error messages
            public const string TaskTypeNotFound = "Task type not found.";

            // Success messages
            public const string CreateSuccess = "Task type has been successfully created.";
            public const string DeleteSuccess = "Task type has been successfully deleted.";
            public const string UpdateSuccess = "Task type has been successfully updated.";
        }

        public static class TaskComments
        {
            // Error messages
            public const string NoPermission = "User does not have permission to edit this comment.";
            public const string BatchNotFound = "No task comments found.";

            // Unauthorized task comments errors
            public const string TaskCommentsViewError = "User does not have sufficient permissions to view tasks in this project.";

            // Success messages
            public const string CreateSuccess = "Task comment has been successfully created.";
            public const string DeleteSuccess = "Task comment has been successfully deleted.";
            public const string UpdateSuccess = "Task comment has been successfully updated.";
        }
    }
}
