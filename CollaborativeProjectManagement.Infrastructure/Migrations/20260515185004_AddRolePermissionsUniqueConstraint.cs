using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionsUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_ProjectRoleId",
                table: "RolePermissions");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ProjectRoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "ProjectRoleId", "PermissionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_ProjectRoleId_PermissionId",
                table: "RolePermissions");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ProjectRoleId",
                table: "RolePermissions",
                column: "ProjectRoleId");
        }
    }
}
