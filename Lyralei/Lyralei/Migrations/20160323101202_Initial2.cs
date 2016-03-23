using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ServerQueryUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServerQueryUser_UserId",
                table: "ServerQueryUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerQueryUser_Users_UserId",
                table: "ServerQueryUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerQueryUser_Users_UserId",
                table: "ServerQueryUser");

            migrationBuilder.DropIndex(
                name: "IX_ServerQueryUser_UserId",
                table: "ServerQueryUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ServerQueryUser");
        }
    }
}
