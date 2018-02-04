﻿// <auto-generated />
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    [DbContext(typeof(DocumentsDbContext))]
    partial class DocumentsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentCheckOutState", b =>
                {
                    b.Property<int>("DocumentId");

                    b.Property<string>("CheckedOutBy")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("CheckOutVersion");

                    b.Property<bool>("CheckedIn");

                    b.Property<string>("CheckedInBy")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<DateTimeOffset>("CheckedOutOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTimeOffset?>("CheckedinOn");

                    b.HasKey("DocumentId", "CheckedOutBy")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("CheckedOutOn")
                        .HasName("IX_DocumentCheckOutState_CheckedOutOn");

                    b.ToTable("DocumentCheckOutStates","documents");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentOwnerState", b =>
                {
                    b.Property<int>("DocumentId");

                    b.Property<string>("Owner")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<DateTimeOffset>("AssociatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("DocumentId", "Owner")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("AssociatedOn")
                        .HasName("IX_DocumentOwnerState_AssociatedOn");

                    b.ToTable("DocumentOwnerStates","documents");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("DocumentStates","documents");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentCheckOutState", b =>
                {
                    b.HasOne("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentState", "Document")
                        .WithMany("CheckOuts")
                        .HasForeignKey("DocumentId")
                        .HasConstraintName("FK_Documents_CheckOuts")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentOwnerState", b =>
                {
                    b.HasOne("Borg.Cms.Basic.PlugIns.Documents.Data.DocumentState", "Document")
                        .WithMany("Owners")
                        .HasForeignKey("DocumentId")
                        .HasConstraintName("FK_Document_Owners")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
