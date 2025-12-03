using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTaskBoard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardMemberships_Boards_BoardId1",
                table: "BoardMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardMemberships_Users_UserId1",
                table: "BoardMemberships");

            migrationBuilder.DropIndex(
                name: "IX_BoardMemberships_BoardId1",
                table: "BoardMemberships");

            migrationBuilder.DropIndex(
                name: "IX_BoardMemberships_UserId1",
                table: "BoardMemberships");

            migrationBuilder.DropColumn(
                name: "BoardId1",
                table: "BoardMemberships");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BoardMemberships");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_TaskItemId",
                table: "Subtasks",
                column: "TaskItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_TaskItems_TaskItemId",
                table: "Subtasks",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_TaskItems_TaskItemId",
                table: "Subtasks");

            migrationBuilder.DropIndex(
                name: "IX_Subtasks_TaskItemId",
                table: "Subtasks");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId1",
                table: "BoardMemberships",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "BoardMemberships",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_BoardId1",
                table: "BoardMemberships",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_UserId1",
                table: "BoardMemberships",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardMemberships_Boards_BoardId1",
                table: "BoardMemberships",
                column: "BoardId1",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardMemberships_Users_UserId1",
                table: "BoardMemberships",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
