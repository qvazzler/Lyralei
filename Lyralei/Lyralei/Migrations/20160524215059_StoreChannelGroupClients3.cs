using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Lyralei.Migrations
{
    public partial class StoreChannelGroupClients3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentChannelId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NeededTalkPower",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NeededSubscribePower",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LatencyFactor",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IconId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeleteDelay",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChannelIconId",
                table: "StoredChannels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<uint>(
                name: "ParentChannelId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "Order",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "NeededTalkPower",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "NeededSubscribePower",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "LatencyFactor",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "IconId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "DeleteDelay",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "ChannelId",
                table: "StoredChannels",
                nullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "ChannelIconId",
                table: "StoredChannels",
                nullable: true);
        }
    }
}
