﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PocketWallet.Data;

namespace PocketWallet.Migrations
{
    [DbContext(typeof(PasswordWalletContext))]
    [Migration("20201120180748_AddBlockForIpAddress")]
    partial class AddBlockForIpAddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PocketWallet.Data.Models.IpAddress", b =>
                {
                    b.Property<string>("FromIpAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("IncorrectSignInCount")
                        .HasColumnType("int");

                    b.Property<bool>("IsPermanentlyBlocked")
                        .HasColumnType("bit");

                    b.HasKey("FromIpAddress");

                    b.ToTable("IpAddresses");
                });

            modelBuilder.Entity("PocketWallet.Data.Models.Password", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WebAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("PocketWallet.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BlockLoginTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("InCorrectLoginCount")
                        .HasColumnType("int");

                    b.Property<bool>("IsPasswordKeptAsHash")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SuccessfulLogin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UnSuccessfulLogin")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasFilter("[Login] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PocketWallet.Data.Models.Password", b =>
                {
                    b.HasOne("PocketWallet.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}