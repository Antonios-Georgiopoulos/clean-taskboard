using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTaskBoard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    BoardId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Boards_BoardId1",
                        column: x => x.BoardId1,
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMemberships_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_BoardId_UserId",
                table: "BoardMemberships",
                columns: new[] { "BoardId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_BoardId1",
                table: "BoardMemberships",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_UserId",
                table: "BoardMemberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberships_UserId1",
                table: "BoardMemberships",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardMemberships");
        }
    }
}
