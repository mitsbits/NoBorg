﻿// <auto-generated />
using Borg.CMS;
using Borg.Platform.EF.CMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Borg.Platform.EF.CMS.data.migrations
{
    [DbContext(typeof(CmsDbContext))]
    partial class CmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("Relational:Sequence:cms.ComponentStatesSQC", "'ComponentStatesSQC', 'cms', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Borg.Platform.EF.CMS.ArticleState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Body")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .IsUnicode(true);

                    b.Property<string>("Slug")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Title")
                        .HasName("IX_Article_Title");

                    b.ToTable("ArticleStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ArticleTagState", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<int>("TagId");

                    b.HasKey("ArticleId", "TagId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTagStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryComponentAssociationState", b =>
                {
                    b.Property<int>("CategoryId");

                    b.Property<int>("ComponentId");

                    b.Property<bool>("IsPrimary");

                    b.HasKey("CategoryId", "ComponentId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("ComponentId");

                    b.ToTable("CategoryComponentAssociationStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryGroupingState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512)
                        .IsUnicode(true);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("CategoryGroupingStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("FriendlyName");

                    b.Property<int>("GroupingId");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("GroupingId");

                    b.ToTable("CategoryStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentDeviceState", b =>
                {
                    b.Property<int>("ComponentId");

                    b.Property<int>("DeviceId");

                    b.HasKey("ComponentId", "DeviceId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("ComponentId")
                        .HasName("IX_ComponentDeviceState_ComponentId");

                    b.HasIndex("DeviceId")
                        .HasName("IX_ComponentDeviceState_DeviceId");

                    b.ToTable("ComponentDeviceStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentDocumentAssociationState", b =>
                {
                    b.Property<int>("ComponentId");

                    b.Property<int>("DocumentId");

                    b.Property<int>("FileId");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("application/octet-stream")
                        .HasMaxLength(256)
                        .IsUnicode(true);

                    b.Property<string>("Uri")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.Property<int>("Version");

                    b.HasKey("ComponentId", "DocumentId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("FileId");

                    b.HasIndex("Version");

                    b.ToTable("ComponentDocumentAssociationStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentJobScheduleState", b =>
                {
                    b.Property<int>("ComponentId");

                    b.Property<int>("ScheduleId");

                    b.HasKey("ComponentId", "ScheduleId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ComponentJobScheduleState","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEXT VALUE FOR cms.ComponentStatesSQC");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPublished");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ComponentStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.DeviceState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("Layout")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("RenderScheme")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("UnSet")
                        .HasMaxLength(512);

                    b.Property<string>("Theme")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("DeviceStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.HtmlSnippetState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.Property<string>("HtmlSnippet")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .IsUnicode(true);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasName("IX_HtmlSnippet_Code");

                    b.ToTable("HtmlSnippetStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.InstanceBlockState", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(2048);

                    b.Property<string>("Display")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("IconClass")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("JsonText");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Display")
                        .HasName("IX_ConfigurationBlock_Display");

                    b.ToTable("ConfigurationBlockStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.NavigationItemState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Display")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.Property<string>("GroupCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(256);

                    b.Property<int>("NavigationItemType");

                    b.Property<string>("Path");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("GroupCode")
                        .HasName("IX_Navigation_GroupCode");

                    b.ToTable("NavigationItemStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.PageMetadataState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("HtmlMetaJsonText")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .IsUnicode(true);

                    b.Property<int?>("PrimaryImageDocumentId");

                    b.Property<int?>("PrimaryImageFileId");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("PageMetadataStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.SectionState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DeviceId");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(512);

                    b.Property<string>("RenderScheme");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("DeviceId")
                        .HasName("IX_Section_DeviceId");

                    b.HasIndex("Identifier")
                        .HasName("IX_Section_Identifier");

                    b.ToTable("SectionStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.SlotState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("ModuleDecriptorJson")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("");

                    b.Property<string>("ModuleGender")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(64);

                    b.Property<string>("ModuleTypeName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024);

                    b.Property<int>("Ordinal");

                    b.Property<int>("SectionId");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("ModuleGender")
                        .HasName("IX_Slot_ModuleGender");

                    b.HasIndex("ModuleTypeName")
                        .HasName("IX_Slot_ModuleTypeName");

                    b.HasIndex("Ordinal")
                        .HasName("IX_Slot_Ordinal");

                    b.HasIndex("SectionId")
                        .HasName("IX_Slot_SectionId");

                    b.ToTable("SlotStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.TagState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.Property<string>("TagNormalized")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.Property<string>("TagSlug")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Tag")
                        .IsUnique()
                        .HasName("IX_Tag_Tag");

                    b.HasIndex("TagNormalized")
                        .IsUnique()
                        .HasName("IX_Tag_TagNormalized");

                    b.ToTable("TagStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.TaxonomyState", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("ArticleId");

                    b.Property<int>("ParentId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<double>("Weight");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("ArticleId")
                        .IsUnique()
                        .HasName("IX_Taxonomy_ArticleId");

                    b.HasIndex("ParentId")
                        .HasName("IX_Taxonomy_ParentId");

                    b.ToTable("TaxonomyStates","cms");
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ArticleState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("Article")
                        .HasForeignKey("Borg.Platform.EF.CMS.ArticleState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ArticleTagState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ArticleState", "Article")
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .HasConstraintName("FK_Articles_ArticleTags")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.TagState", "Tag")
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_Tags_ArticleTags")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryComponentAssociationState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.CategoryState", "Category")
                        .WithMany("CategoryComponentAssociations")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Categories_CategoryComponentAssociation")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithMany("CategoryComponentAssociations")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_Components_CategoryComponentAssociation")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryGroupingState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("CategoryGrouping")
                        .HasForeignKey("Borg.Platform.EF.CMS.CategoryGroupingState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.CategoryState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.CategoryGroupingState", "Grouping")
                        .WithMany("Categories")
                        .HasForeignKey("GroupingId")
                        .HasConstraintName("FK_CategoryGroupings_Categories")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("Category")
                        .HasForeignKey("Borg.Platform.EF.CMS.CategoryState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.TaxonomyState", "Taxonomy")
                        .WithOne("Category")
                        .HasForeignKey("Borg.Platform.EF.CMS.CategoryState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentDeviceState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("ComponentDevice")
                        .HasForeignKey("Borg.Platform.EF.CMS.ComponentDeviceState", "ComponentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.DeviceState", "Device")
                        .WithOne("ComponentDevice")
                        .HasForeignKey("Borg.Platform.EF.CMS.ComponentDeviceState", "DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentDocumentAssociationState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithMany("ComponentDocumentAssociations")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_Components_Documents")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.ComponentJobScheduleState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithMany("ComponentJobSchedules")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_Component_JobSchedules")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.HtmlSnippetState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("HtmlSnippet")
                        .HasForeignKey("Borg.Platform.EF.CMS.HtmlSnippetState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.NavigationItemState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.TaxonomyState", "Taxonomy")
                        .WithOne("NavigationItem")
                        .HasForeignKey("Borg.Platform.EF.CMS.NavigationItemState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.PageMetadataState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ArticleState", "Article")
                        .WithOne("PageMetadata")
                        .HasForeignKey("Borg.Platform.EF.CMS.PageMetadataState", "Id")
                        .HasConstraintName("FK_Articles_PageMetadatas")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("PageMetadata")
                        .HasForeignKey("Borg.Platform.EF.CMS.PageMetadataState", "Id")
                        .HasConstraintName("FK_Components_PageMetadatas")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.SectionState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.DeviceState", "Device")
                        .WithMany("Sections")
                        .HasForeignKey("DeviceId")
                        .HasConstraintName("FK_Device_Section")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.SlotState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.SectionState", "Section")
                        .WithMany("Slots")
                        .HasForeignKey("SectionId")
                        .HasConstraintName("FK_Section_Slot")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.TagState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("Tag")
                        .HasForeignKey("Borg.Platform.EF.CMS.TagState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Platform.EF.CMS.TaxonomyState", b =>
                {
                    b.HasOne("Borg.Platform.EF.CMS.ArticleState", "Article")
                        .WithOne("Taxonomy")
                        .HasForeignKey("Borg.Platform.EF.CMS.TaxonomyState", "ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Platform.EF.CMS.ComponentState", "Component")
                        .WithOne("Taxonomy")
                        .HasForeignKey("Borg.Platform.EF.CMS.TaxonomyState", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
