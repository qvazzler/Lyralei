using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Lyralei;

namespace Lyralei.Migrations
{
    [DbContext(typeof(CoreContext))]
    [Migration("20160523133911_SomeTest2")]
    partial class SomeTest2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc2-16649")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lyralei.Core.InputOwner.Models.InputOwners", b =>
                {
                    b.ToTable("InputOwners");

                    b.Property<int>("InputOwnersId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserId");

                    b.HasKey("InputOwnersId");

                    b.HasIndex("UserId");
                });

            modelBuilder.Entity("Lyralei.Core.PermissionManager.Models.UserPermissions", b =>
                {
                    b.ToTable("UserPermissions");

                    b.Property<int>("UserPermissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PermissionName");

                    b.Property<int?>("PermissionValue");

                    b.Property<int>("UserId");

                    b.HasKey("UserPermissionId");

                    b.HasIndex("UserId");
                });

            modelBuilder.Entity("Lyralei.Core.ServerQueryConnection.Models.Subscribers", b =>
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

                    b.HasIndex("SubscriberUniqueId")
                        .IsUnique();
                });

            modelBuilder.Entity("Lyralei.Core.ServerQueryShell.Models.ServerQueryUser", b =>
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

            modelBuilder.Entity("Lyralei.Core.UserManager.Models.Users", b =>
                {
                    b.ToTable("Users");

                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubscriberId");

                    b.Property<string>("SubscriberUniqueId");

                    b.Property<string>("UserTeamSpeakClientUniqueId");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("Lyralei.Core.InputOwner.Models.InputOwners", b =>
                {
                    b.HasOne("Lyralei.Core.UserManager.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lyralei.Core.PermissionManager.Models.UserPermissions", b =>
                {
                    b.HasOne("Lyralei.Core.UserManager.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lyralei.Core.ServerQueryShell.Models.ServerQueryUser", b =>
                {
                    b.HasOne("Lyralei.Core.UserManager.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lyralei.Core.Test.Models.TestUser", b =>
                {
                    b.HasOne("Lyralei.Core.UserManager.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
