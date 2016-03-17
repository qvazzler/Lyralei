using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Lyralei;

namespace Lyralei.Migrations
{
    [DbContext(typeof(AlleriaContext))]
    partial class AlleriaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc2-16649")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lyralei.Models.Subscribers", b =>
                {
                    b.ToTable("Subscribers");

                    b.Property<int>("SubscriberId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminPassword");

                    b.Property<string>("AdminUsername");

                    b.Property<string>("ServerIp")
                        .IsRequired();

                    b.Property<short>("ServerPort");

                    b.Property<int>("VirtualServerId");

                    b.HasKey("SubscriberId");
                });
        }
    }
}
