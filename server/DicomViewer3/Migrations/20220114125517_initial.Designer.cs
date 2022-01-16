﻿// <auto-generated />
using System;
using DicomViewer3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DicomViewer3.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220114125517_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DicomViewer3.Entities.Instance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("MongoId")
                        .HasColumnType("text");

                    b.Property<long>("OriginalId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SeriesId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("Instances");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Series", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Modality")
                        .HasColumnType("text");

                    b.Property<string>("OriginalId")
                        .HasColumnType("text");

                    b.Property<long?>("StudyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StudyId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Study", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("OriginalId")
                        .HasColumnType("text");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("DicomViewer3.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Instance", b =>
                {
                    b.HasOne("DicomViewer3.Entities.Series", "Series")
                        .WithMany("Instances")
                        .HasForeignKey("SeriesId");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Series", b =>
                {
                    b.HasOne("DicomViewer3.Entities.Study", "Study")
                        .WithMany()
                        .HasForeignKey("StudyId");

                    b.Navigation("Study");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Study", b =>
                {
                    b.HasOne("DicomViewer3.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DicomViewer3.Entities.Series", b =>
                {
                    b.Navigation("Instances");
                });
#pragma warning restore 612, 618
        }
    }
}
