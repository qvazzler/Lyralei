using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class AddedUsers6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users",
                nullable: false);
        }
    }
}
