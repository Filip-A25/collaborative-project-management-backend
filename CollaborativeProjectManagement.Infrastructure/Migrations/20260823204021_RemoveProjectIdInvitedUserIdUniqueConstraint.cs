using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectIdInvitedUserIdUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectInvites_ProjectId_InvitedUserId",
                table: "ProjectInvites");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvites_ProjectId",
                table: "ProjectInvites",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectInvites_ProjectId",
                table: "ProjectInvites");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvites_ProjectId_InvitedUserId",
                table: "ProjectInvites",
                columns: new[] { "ProjectId", "InvitedUserId" },
                unique: true);
        }
    }
}
