using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Lyralei;

namespace Lyralei.Migrations
{
    [DbContext(typeof(CoreContext))]
    [Migration("20160323203450_TestAddon1")]
    partial class TestAddon1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc2-16649")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lyralei.Core.ServerQuery.Models.ServerQueryUser", b =>
                {
                    b.ToTable("ServerQueryUser");

                    b.Property<int>("ServerQueryUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ServerQueryPassword");

                    b.Property<string>("ServerQueryUsername");

                    b.Property<int>("UserId");

                    b.HasKey("ServerQueryUserId");

                    b.HasIndex("UserId");
                });

            modelBuilder.Entity("Lyralei.Core.Test.Models.TestUser", b =>
                {
                    b.ToTable("TestUser");

                    b.Property<int>("TestUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RequiredFieldHere")
                        .IsRequired();

                    b.Property<string>("SomeTestInformation");

                    b.Property<int>("UserId");

                    b.HasKey("TestUserId");

                    b.HasIndex("UserId");
                });

            modelBuilder.Entity("Lyralei.Models.Subscribers", b =>
                {
                    b.ToTable("Subscribers");

                    b.Property<int>("SubscriberId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminPassword")
                        .IsRequired();

                    b.Property<string>("AdminUsername")
                        .IsRequired();

                    b.Property<string>("ServerIp")
                        .IsRequired();

                    b.Property<short>("ServerPort");

                    b.Property<string>("SubscriberUniqueId");

                    b.Property<int>("VirtualServerId");

                    b.HasKey("SubscriberId");
                });

            modelBuilder.Entity("Lyralei.Models.Users", b =>
                {
                    b.ToTable("Users");

                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubscriberId");

                    b.Property<string>("SubscriberUniqueId");

                    b.Property<string>("UserTeamSpeakClientUniqueId");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("Lyralei.Core.ServerQuery.Models.ServerQueryUser", b =>
                {
                    b.HasOne("Lyralei.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lyralei.Core.Test.Models.TestUser", b =>
                {
                    b.HasOne("Lyralei.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
