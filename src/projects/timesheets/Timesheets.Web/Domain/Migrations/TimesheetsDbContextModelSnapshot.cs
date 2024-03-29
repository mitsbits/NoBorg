﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Timesheets.Web.Domain;

namespace Web.Domain.Migrations
{
    [DbContext(typeof(TimesheetsDbContext))]
    partial class TimesheetsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Timesheets.Web.Domain.AnnualVacation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Days");

                    b.Property<Guid>("WorkerId");

                    b.Property<string>("WorkerId1");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("WorkerId1");

                    b.ToTable("AnnualVacation");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.AspRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.AspUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.AspUserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Span");

                    b.Property<Guid>("TaxonomyId");

                    b.Property<Guid>("WorkingDayId");

                    b.HasKey("Id");

                    b.HasIndex("WorkingDayId");

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.BankHoliday", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Date");

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<string>("TeamId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("BankHolidays");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Taxonomy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsEnabled");

                    b.Property<Guid?>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("Taxonomies");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.TaxonomyTag", b =>
                {
                    b.Property<Guid>("TaxonomyId");

                    b.Property<Guid>("TagId");

                    b.HasKey("TaxonomyId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("TaxonomiesTags");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Team", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("TimeZoneInfoId")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Worker", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .HasMaxLength(512);

                    b.Property<string>("LastName")
                        .HasMaxLength(512);

                    b.Property<string>("TeamId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.WorkingDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Date");

                    b.Property<string>("WorkerId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("WorkerId");

                    b.ToTable("WorkingDays");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.AnnualVacation", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.Worker", "Worker")
                        .WithMany("AnnualVacations")
                        .HasForeignKey("WorkerId1");
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Assignment", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.WorkingDay", "WorkingDay")
                        .WithMany("Assignments")
                        .HasForeignKey("WorkingDayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Timesheets.Web.Domain.BankHoliday", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.Team", "Team")
                        .WithMany("BankHolidays")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Timesheets.Web.Domain.TaxonomyTag", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Timesheets.Web.Domain.Taxonomy", "Taxonomy")
                        .WithMany("Tags")
                        .HasForeignKey("TaxonomyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Timesheets.Web.Domain.Worker", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Timesheets.Web.Domain.WorkingDay", b =>
                {
                    b.HasOne("Timesheets.Web.Domain.Worker")
                        .WithMany("WorkingDays")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
