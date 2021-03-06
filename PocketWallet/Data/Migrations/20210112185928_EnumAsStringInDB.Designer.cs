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
    [Migration("20210112185928_EnumAsStringInDB")]
    partial class EnumAsStringInDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PocketWallet.Data.Models.DataChange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RecordId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DataChanges");
                });

            modelBuilder.Entity("PocketWallet.Data.Models.Function", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Functions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("82faf31b-b754-41d2-9ac2-742643f96503"),
                            Description = "User sign in to application",
                            Name = "SignIn"
                        },
                        new
                        {
                            Id = new Guid("660cea17-8c94-4d2b-add4-655514d8f998"),
                            Description = "User create new account in application",
                            Name = "SignUp"
                        },
                        new
                        {
                            Id = new Guid("cef9db47-dbb8-4655-a741-1cc394007e0c"),
                            Description = "User change his master password",
                            Name = "ChangeMasterPassword"
                        },
                        new
                        {
                            Id = new Guid("8fea7f6e-54d3-4915-ae34-7819bb980ca1"),
                            Description = "User gets his sign in information (i.e successful login time)",
                            Name = "GetLoginInfo"
                        },
                        new
                        {
                            Id = new Guid("cd1e512d-baa6-41f6-95b9-20000ad7e279"),
                            Description = "User add new password to his wallet",
                            Name = "AddPassword"
                        },
                        new
                        {
                            Id = new Guid("bdecbb1e-9d0f-4d32-9650-e09f5aa21fc2"),
                            Description = "User delete password from his wallet",
                            Name = "DeletePassword"
                        },
                        new
                        {
                            Id = new Guid("3264732d-3ac6-42ba-af99-5841b6d830ed"),
                            Description = "User edit password in his wallet",
                            Name = "EditPassword"
                        },
                        new
                        {
                            Id = new Guid("19ac49d8-1b0d-4c16-a89e-9efabfb165d9"),
                            Description = "Get password record without password value",
                            Name = "GetFullSecurityPassword"
                        },
                        new
                        {
                            Id = new Guid("82e8c449-4ad7-4067-b181-05447c161fb3"),
                            Description = "Get password decrypted value",
                            Name = "GetPassword"
                        },
                        new
                        {
                            Id = new Guid("7716e6a3-43d3-4a72-9aea-179fb60a7a57"),
                            Description = "Get all wallet",
                            Name = "GetWallet"
                        },
                        new
                        {
                            Id = new Guid("ba46cc4f-25fd-4efb-8b12-7dce0032a57c"),
                            Description = "Share password record to other user",
                            Name = "SharePassword"
                        });
                });

            modelBuilder.Entity("PocketWallet.Data.Models.FunctionRun", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FunctionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FunctionId");

                    b.HasIndex("UserId");

                    b.ToTable("FunctionRuns");
                });

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

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

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

            modelBuilder.Entity("PocketWallet.Data.Models.SharedPassword", b =>
                {
                    b.Property<Guid>("PasswordId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PasswordId1")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PasswordId", "UserId");

                    b.HasIndex("PasswordId1");

                    b.HasIndex("UserId");

                    b.ToTable("SharedPasswords");
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

            modelBuilder.Entity("PocketWallet.Data.Models.DataChange", b =>
                {
                    b.HasOne("PocketWallet.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PocketWallet.Data.Models.FunctionRun", b =>
                {
                    b.HasOne("PocketWallet.Data.Models.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PocketWallet.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PocketWallet.Data.Models.Password", b =>
                {
                    b.HasOne("PocketWallet.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PocketWallet.Data.Models.SharedPassword", b =>
                {
                    b.HasOne("PocketWallet.Data.Models.Password", "Password")
                        .WithMany()
                        .HasForeignKey("PasswordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PocketWallet.Data.Models.Password", null)
                        .WithMany("SharedPasswords")
                        .HasForeignKey("PasswordId1");

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
