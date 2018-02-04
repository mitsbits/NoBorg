﻿// <auto-generated />
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.Assets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    [DbContext(typeof(AssetsDbContext))]
    partial class AssetsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("Relational:Sequence:assets.AssetsSQC", "'AssetsSQC', 'assets', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("Relational:Sequence:assets.FilesSQC", "'FilesSQC', 'assets', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("Relational:Sequence:assets.VersionsSQC", "'VersionsSQC', 'assets', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Borg.Platform.EF.Assets.AssetRecord", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValueSql("NEXT VALUE FOR assets.AssetsSQC");

                    b.Property<int>("CurrentVersion")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int>("DocumentBehaviourState");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("AssetRecords","assets");
                });

            modelBuilder.Entity("Borg.Platform.EF.Assets.FileRecord", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValueSql("NEXT VALUE FOR assets.FilesSQC");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024);

                    b.Property<DateTime?>("LastRead");

                    b.Property<DateTime>("LastWrite")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<long>("SizeInBytes")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("FullPath")
                        .HasName("IX_File_FullPath");

                    b.ToTable("FileRecords","assets");
                });

            modelBuilder.Entity("Borg.Platform.EF.Assets.VersionRecord", b =>
                {
                    b.Property<int>("Id")
                        .HasDefaultValueSql("NEXT VALUE FOR assets.VersionsSQC");

                    b.Property<int>("AssetRecordId");

                    b.Property<int>("FileRecordId");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("FileRecordId")
                        .IsUnique()
                        .HasName("IX_Version_FileRecordId");

                    b.HasIndex("Version")
                        .HasName("IX_Version_Version");

                    b.HasIndex("AssetRecordId", "Version")
                        .IsUnique()
                        .HasName("PK_Version_Asset");

                    b.ToTable("VersionRecords","assets");
                });

            modelBuilder.Entity("Borg.Platform.EF.Assets.VersionRecord", b =>
                {
                    b.HasOne("Borg.Platform.EF.Assets.AssetRecord", "AssetRecord")
                        .WithMany("Versions")
                        .HasForeignKey("AssetRecordId")
                        .HasConstraintName("FK_Asset_Version")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.Assets.FileRecord", "FileRecord")
                        .WithOne("VersionRecord")
                        .HasForeignKey("Borg.Platform.EF.Assets.VersionRecord", "FileRecordId")
                        .HasConstraintName("FK_Version_File")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
