using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class AddedUsers4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserTeamSpeakClientId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserTeamSpeakClientUniqueId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserTeamSpeakClientId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserTeamSpeakClientUniqueId",
                table: "Users");
        }
    }
}
