using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class AddedUsers3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ServerQueryPassword",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "ServerQueryPassword",
                table: "Users",
                nullable: false);
        }
    }
}
