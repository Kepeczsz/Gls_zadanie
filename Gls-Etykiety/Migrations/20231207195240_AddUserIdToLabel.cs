using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gls_Etykiety.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToLabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Labels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Labels_UserId",
                table: "Labels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Users_UserId",
                table: "Labels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Users_UserId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_UserId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Labels");
        }
    }
}
