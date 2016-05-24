using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class StoreChannelGroupClients2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClientDatabaseId",
                table: "StoredChannelGroupClients",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "StoredChannelGroupClients",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelGroupId",
                table: "StoredChannelGroupClients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "ClientDatabaseId",
                table: "StoredChannelGroupClients",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "ChannelId",
                table: "StoredChannelGroupClients",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "ChannelGroupId",
                table: "StoredChannelGroupClients",
                nullable: true);
        }
    }
}
