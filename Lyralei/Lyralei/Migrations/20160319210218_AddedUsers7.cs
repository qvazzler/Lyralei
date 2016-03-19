using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class AddedUsers7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SubscriberUniqueId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriberUniqueId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserTeamSpeakClientDatabaseId",
                table: "Users",
                nullable: true);
        }
    }
}
