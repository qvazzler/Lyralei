using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class AddedUsers5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTeamSpeakClientId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTeamSpeakClientId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
