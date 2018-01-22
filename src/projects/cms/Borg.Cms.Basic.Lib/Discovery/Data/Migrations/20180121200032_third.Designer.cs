﻿// <auto-generated />
using Borg.Cms.Basic.Lib.Discovery.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Borg.Cms.Basic.Lib.Discovery.Data.Migrations
{
    [DbContext(typeof(DiscoveryDbContext))]
    [Migration("20180121200032_third")]
    partial class third
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Blog","discovery");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blogger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Slug");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Blogger","discovery");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.BloggerBlog", b =>
                {
                    b.Property<int>("BlogId");

                    b.Property<int>("BloggerId");

                    b.HasKey("BlogId", "BloggerId");

                    b.HasIndex("BloggerId");

                    b.ToTable("BloggerBlog","discovery");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blogpost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<int>("BloggerId");

                    b.Property<string>("Body");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("BloggerId");

                    b.ToTable("Blogpost","discovery");
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.BloggerBlog", b =>
                {
                    b.HasOne("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blog")
                        .WithMany("BloggerBlogs")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blogger")
                        .WithMany("BloggerBlogs")
                        .HasForeignKey("BloggerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blogpost", b =>
                {
                    b.HasOne("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blog", "Blog")
                        .WithMany()
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Borg.Cms.Basic.PlugIns.BlogEngine.Domain.Blogger", "Blogger")
                        .WithMany()
                        .HasForeignKey("BloggerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}