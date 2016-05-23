using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace Lyralei.Migrations
{
    public partial class addNickNametoSubscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelDesignations",
                columns: table => new
                {
                    ChannelDesignationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<int>(nullable: false),
                    DesignatedByUserId = table.Column<int>(nullable: false),
                    DesignationName = table.Column<string>(nullable: true),
                    SubscriberId = table.Column<int>(nullable: false),
                    SubscriberUniqueId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelDesignations", x => x.ChannelDesignationId);
                });

            migrationBuilder.AddColumn<string>(
                name: "BotNickName",
                table: "Subscribers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BotNickName",
                table: "Subscribers");

            migrationBuilder.DropTable(
                name: "ChannelDesignations");
        }
    }
}
