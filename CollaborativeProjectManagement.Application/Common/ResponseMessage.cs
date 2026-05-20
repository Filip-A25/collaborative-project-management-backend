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
    }
}
