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
    [Migration("20201211170316_AddCustomLogs")]
    partial class AddCustomLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                            Id = new Guid("19af78d7-5ced-4abc-a2a4-cdd1fe218786"),
                            Description = "User sign in to application",
                            Name = "SignIn"
                        },
                        new
                        {
                            Id = new Guid("2e9c3c39-b363-4071-8da7-1369dbcbd68a"),
                            Description = "User create new account in application",
                            Name = "SignUp"
                        },
                        new
                        {
                            Id = new Guid("9439daf7-40d8-4bed-8447-c74e6ae8ccaa"),
                            Description = "User change his master password",
                            Name = "ChangeMasterPassword"
                        },
                        new
                        {
                            Id = new Guid("61d96923-635e-42e8-aa19-b1c7837ed30b"),
                            Description = "User gets his sign in information (i.e successful login time)",
                            Name = "GetLoginInfo"
                        },
                        new
                        {
                            Id = new Guid("eafe5933-128c-4cb5-b1d1-70665edcadd8"),
                            Description = "User add new password to his wallet",
                            Name = "AddPassword"
                        },
                        new
                        {
                            Id = new Guid("3128d37d-53f2-4888-9cfa-85757aa2466d"),
                            Description = "User delete password from his wallet",
                            Name = "DeletePassword"
                        },
                        new
                        {
                            Id = new Guid("7f6be03a-8d1e-40f7-afbc-0ac557efcbbc"),
                            Description = "User edit password in his wallet",
                            Name = "EditPassword"
                        },
                        new
                        {
                            Id = new Guid("e2b61fd4-5cfd-4eac-837e-fcb4410dc8ca"),
                            Description = "Get password record without password value",
                            Name = "GetFullSecurityPassword"
                        },
                        new
                        {
                            Id = new Guid("a0427735-ef4e-41e3-8008-959f4f85aa5e"),
                            Description = "Get password decrypted value",
                            Name = "GetPassword"
                        },
                        new
                        {
                            Id = new Guid("0545ebdc-6305-4077-8e2c-4318e01b9a38"),
                            Description = "Get all wallet",
                            Name = "GetWallet"
                        },
                        new
                        {
                            Id = new Guid("a8d49ccb-267a-4424-ab69-c999c72482d1"),
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
