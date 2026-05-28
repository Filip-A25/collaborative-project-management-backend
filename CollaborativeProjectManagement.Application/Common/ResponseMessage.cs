namespace CollaborativeProjectManagement.Application.Common
{
    public static class ResponseMessage
    {
        public static class Auth
        {
            // Error messages
            public const string InternalRegisterError = "Something went wrong while trying to register.";
            public const string InternalLoginError = "Something went wrong while trying to login.";
            public const string RegisterConflict = "An account with this email already exists.";
            public const string UserNotFound = "User could not be found.";
            public const string IncorrectPassword = "Incorrect password entered.";

            // Success messages
            public const string RegisterSuccess = "User has been successfully registered.";
            public const string LoginSuccess = "User has successfully logged in.";
        }

        public static class Projects
        {
            // Error messages
            public const string InternalCreateError = "Something went wrong while creating a project.";
            public const string InternalDeleteError = "Something went wrong while trying to delete the project.";
            public const string InternalFetchError = "Something went wrong while trying to fetch the project.";
            public const string InternalBatchFetchError = "Something went wrong while trying to fetch the projects.";
            public const string AuthorizationError = "User has to be an Admin to create a project.";
            public const string ProjectNotFound = "Project could not be found.";
            public const string ProjectsDontExist = "User does not have any projects.";

            // Unauthorized role errors
            public const string ProjectRoleDeleteError = "User does not have sufficient permissions to delete the project.";
            public const string ProjectRoleViewError = "User does not have sufficient permissions to view the project.";
            public const string ProjectManageError = "User does not have sufficient permissions to manage this project.";

            // Success messages
            public const string DeleteSuccess = "Project has been successfully deleted.";
        }

        public static class ProjectRoles
        {
            // Error messages
            public const string InternalCreateError = "Something went wrong while trying to create a project role.";
            public const string InternalDeleteError = "Something went wrong while trying to delete a project role.";
            public const string InternalCreatePermissionsError = "Something went wrong while trying to add permissions to a project role.";
            public const string RoleNotFound = "Role could not be found.";

            // Unauthorized role errors
            public const string RolesManageError = "User does not have sufficient permissions to manage project roles.";

            // Success messages
            public const string AddPermissionsSuccess = "Successfully added permissions to the role.";
            public const string DeleteBatchSuccess = "Project roles have been successfully deleted.";
        }

        public static class ProjectInvites
        {
            // Error messages
            public const string InternalCreateError = "Something went wrong while trying to invite members to a project.";
            public const string UsersNotFound = "Users could not be found.";
            public const string InternalUpdateError = "Something went wrong while trying to update invite accepted state.";
            public const string InternalDeleteError = "Something went wrong while trying to delete a project invite.";
            public const string Expired = "Unable to update the invite. Invite has expired.";
            public const string InternalFetchError = "Something went wrong while trying to fetch the invites.";

            // Unauthorized invites errors
            public const string InvitesCreateError = "User does not have sufficient permissions to create invites.";
            public const string InvitesDeleteError = "User does not have sufficient permissions to delete invites.";
            public const string InvitesFetchError = "User does not have sufficient permissions to fetch project invites.";

            // Success messages
            public const string CreateSuccess = "User has been invited to a project.";
            public const string DeleteSuccess = "Invite has been successfully deleted.";
            public const string AcceptSuccess = "Invite has been successfully accepted.";
        }

        public static class Tasks
        {
            // Error messages
            public const string TasksManageError = "User does not have sufficient permissions to manage tasks.";
            public const string InternalCreateError = "Something went wrong while trying to create a task.";
            public const string InternalDeleteError = "Something went wrong while trying to delete a task.";
            public const string InternalFetchError = "Something went wrong while trying to fetch tasks.";
            public const string ProjectTasksNotFound = "No tasks were found for the project.";
            public const string MemberNotFound = "Assigned user is not a member in this project.";
            public const string TaskNotFound = "Task not found.";
            public const string InternalTypeCreateError = "Something went wrong while trying to create a task type.";

            // Unauthorized tasks errros
            public const string ViewTasksError = "User does not have sufficient permissions to view tasks in this project.";

            // Success messages
            public const string CreateSuccess = "Task has been successfully created.";
            public const string DeleteSuccess = "Task has been successfully deleted.";
        }
    }
}
