using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerQueryPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ServerQueryUsername",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ServerQueryUserDetailsForeignKey",
                table: "ServerQueryUserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "ServerQueryUserDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerQueryUserDetails_UsersUserId",
                table: "ServerQueryUserDetails",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerQueryUserDetails_Users_UsersUserId",
                table: "ServerQueryUserDetails",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerQueryUserDetails_Users_UsersUserId",
                table: "ServerQueryUserDetails");

            migrationBuilder.DropIndex(
                name: "IX_ServerQueryUserDetails_UsersUserId",
                table: "ServerQueryUserDetails");

            migrationBuilder.DropColumn(
                name: "ServerQueryUserDetailsForeignKey",
                table: "ServerQueryUserDetails");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "ServerQueryUserDetails");

            migrationBuilder.AddColumn<string>(
                name: "ServerQueryPassword",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServerQueryUsername",
                table: "Users",
                nullable: true);
        }
    }
}
