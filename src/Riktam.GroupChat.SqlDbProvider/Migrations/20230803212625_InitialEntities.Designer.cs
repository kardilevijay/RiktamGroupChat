﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;

#nullable disable

namespace Riktam.GroupChat.SqlDbProvider.Migrations;

[DbContext(typeof(GroupChatDbContext))]
[Migration("20230803212625_InitialEntities")]
partial class InitialEntities
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.9")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.Group", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("tblGroups");
            });

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.GroupMessage", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("GroupId")
                    .HasColumnType("int");

                b.Property<string>("Message")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)");

                b.Property<DateTime>("Timestamp")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("GroupId");

                b.HasIndex("UserId");

                b.ToTable("tblGroupMessages");
            });

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Password")
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnType("nvarchar(512)");

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("tblUsers");
            });

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.GroupMessage", b =>
            {
                b.HasOne("Riktam.GroupChat.SqlDbProvider.Models.Group", "Group")
                    .WithMany("GroupMessages")
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Riktam.GroupChat.SqlDbProvider.Models.User", "User")
                    .WithMany("GroupMessages")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Group");

                b.Navigation("User");
            });

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.Group", b =>
            {
                b.Navigation("GroupMessages");
            });

        modelBuilder.Entity("Riktam.GroupChat.SqlDbProvider.Models.User", b =>
            {
                b.Navigation("GroupMessages");
            });
#pragma warning restore 612, 618
    }
}
