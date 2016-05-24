using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace Lyralei.Migrations
{
    public partial class StoreChannelGroupClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoredChannelGroupClients",
                columns: table => new
                {
                    StoredChannelGroupClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelGroupId = table.Column<uint>(nullable: true),
                    ChannelId = table.Column<uint>(nullable: true),
                    ClientDatabaseId = table.Column<uint>(nullable: true),
                    SubscriberId = table.Column<int>(nullable: false),
                    SubscriberUniqueId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredChannelGroupClients", x => x.StoredChannelGroupClientId);
                });

            migrationBuilder.CreateTable(
                name: "StoredChannels",
                columns: table => new
                {
                    StoredChannelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelIconId = table.Column<uint>(nullable: true),
                    ChannelId = table.Column<uint>(nullable: true),
                    Codec = table.Column<ushort>(nullable: true),
                    CodecQuality = table.Column<double>(nullable: true),
                    DeleteDelay = table.Column<uint>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    ForcedSilence = table.Column<bool>(nullable: true),
                    IconId = table.Column<uint>(nullable: true),
                    IsDefaultChannel = table.Column<bool>(nullable: true),
                    IsMaxClientsUnlimited = table.Column<bool>(nullable: true),
                    IsMaxFamilyClientsInherited = table.Column<bool>(nullable: true),
                    IsMaxFamilyClientsUnlimited = table.Column<bool>(nullable: true),
                    IsPasswordProtected = table.Column<bool>(nullable: true),
                    IsPermanent = table.Column<bool>(nullable: true),
                    IsSemiPermanent = table.Column<bool>(nullable: true),
                    IsSpacer = table.Column<bool>(nullable: true),
                    IsUnencrypted = table.Column<bool>(nullable: true),
                    LatencyFactor = table.Column<uint>(nullable: true),
                    MaxClients = table.Column<int>(nullable: true),
                    MaxFamilyClients = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NeededSubscribePower = table.Column<uint>(nullable: true),
                    NeededTalkPower = table.Column<uint>(nullable: true),
                    Order = table.Column<uint>(nullable: true),
                    ParentChannelId = table.Column<uint>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneticName = table.Column<string>(nullable: true),
                    SecondsEmpty = table.Column<int>(nullable: true),
                    SecuritySalt = table.Column<string>(nullable: true),
                    StoredChannelUniqueId = table.Column<string>(nullable: true),
                    SubscriberId = table.Column<int>(nullable: false),
                    SubscriberUniqueId = table.Column<string>(nullable: true),
                    Topic = table.Column<string>(nullable: true),
                    TotalClients = table.Column<int>(nullable: true),
                    TotalClientsFamily = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredChannels", x => x.StoredChannelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredChannelGroupClients");

            migrationBuilder.DropTable(
                name: "StoredChannels");
        }
    }
}
