﻿// <auto-generated />

using System;
using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SPMS.Persistence.Migrations
{
    [DbContext(typeof(SpmsContext))]
    [Migration("20200613205942_LinkEpisodeToSeries2")]
    partial class LinkEpisodeToSeries2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SPMS.Web.Models.Biography", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Affiliation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Assignment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Born")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Eyes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hair")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Homeworld")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PostingId")
                        .HasColumnType("int");

                    b.Property<string>("Rank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RankImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Weight")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PostingId");

                    b.HasIndex("StatusId");

                    b.ToTable("Biography");
                });

            modelBuilder.Entity("SPMS.Web.Models.BiographyState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BiographyState");
                });

            modelBuilder.Entity("SPMS.Web.Models.Episode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.HasIndex("StatusId");

                    b.ToTable("Episode");
                });

            modelBuilder.Entity("SPMS.Web.Models.EpisodeEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EpisodeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("EpisodeId");

                    b.ToTable("EpisodeEntry");
                });

            modelBuilder.Entity("SPMS.Web.Models.EpisodeStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EpisodeStatus");
                });

            modelBuilder.Entity("SPMS.Web.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("SPMS.Web.Models.Posting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Posting");
                });

            modelBuilder.Entity("SPMS.Web.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("SPMS.Web.Models.Biography", b =>
                {
                    b.HasOne("SPMS.Web.Models.Posting", "Posting")
                        .WithMany()
                        .HasForeignKey("PostingId");

                    b.HasOne("SPMS.Web.Models.BiographyState", "State")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("SPMS.Web.Models.Episode", b =>
                {
                    b.HasOne("SPMS.Web.Models.Series", "Series")
                        .WithMany("Episodes")
                        .HasForeignKey("SeriesId");

                    b.HasOne("SPMS.Web.Models.EpisodeStatus", "State")
                        .WithMany()
                        .HasForeignKey("StatusId");
                });

            modelBuilder.Entity("SPMS.Web.Models.EpisodeEntry", b =>
                {
                    b.HasOne("SPMS.Web.Models.Episode", null)
                        .WithMany("Entries")
                        .HasForeignKey("EpisodeId");
                });
#pragma warning restore 612, 618
        }
    }
}
