using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectTasksTablesOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProjectMembers_CommenterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_AssignedTo",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_CreatorId",
                table: "ProjectTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProjectMembers_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_AssignedTo",
                table: "ProjectTasks",
                column: "AssignedTo",
                principalTable: "ProjectMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_CreatorId",
                table: "ProjectTasks",
                column: "CreatorId",
                principalTable: "ProjectMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProjectMembers_CommenterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_AssignedTo",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_CreatorId",
                table: "ProjectTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProjectMembers_CommenterId",
                table: "Comments",
                column: "CommenterId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProjectTasks_ProjectTaskId",
                table: "Comments",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_AssignedTo",
                table: "ProjectTasks",
                column: "AssignedTo",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_CreatorId",
                table: "ProjectTasks",
                column: "CreatorId",
                principalTable: "ProjectMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
