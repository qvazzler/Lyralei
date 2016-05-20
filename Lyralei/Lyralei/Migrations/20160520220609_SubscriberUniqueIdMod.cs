using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class SubscriberUniqueIdMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubscriberUniqueId",
                table: "Subscribers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_SubscriberUniqueId",
                table: "Subscribers",
                column: "SubscriberUniqueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscribers_SubscriberUniqueId",
                table: "Subscribers");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriberUniqueId",
                table: "Subscribers",
                nullable: true);
        }
    }
}
