using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativeProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInviterUserIdToProjectInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InviterUserId",
                table: "ProjectInvites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvites_InviterUserId",
                table: "ProjectInvites",
                column: "InviterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectInvites_Users_InviterUserId",
                table: "ProjectInvites",
                column: "InviterUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectInvites_Users_InviterUserId",
                table: "ProjectInvites");

            migrationBuilder.DropIndex(
                name: "IX_ProjectInvites_InviterUserId",
                table: "ProjectInvites");

            migrationBuilder.DropColumn(
                name: "InviterUserId",
                table: "ProjectInvites");
        }
    }
}
