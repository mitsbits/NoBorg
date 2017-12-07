using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Web.Domain;

namespace Timesheets.Web.Domain.Migrations
{
    [DbContext(typeof(TimesheetsDbContext))]
    [Migration("20170722115822_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Web.Domain.AnnualVacation", b =>
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

            modelBuilder.Entity("Web.Domain.AspRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Web.Domain.AspUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Web.Domain.AspUserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Web.Domain.Assignment", b =>
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

            modelBuilder.Entity("Web.Domain.BankHoliday", b =>
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

            modelBuilder.Entity("Web.Domain.Team", b =>
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

            modelBuilder.Entity("Web.Domain.Worker", b =>
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

            modelBuilder.Entity("Web.Domain.WorkingDay", b =>
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

            modelBuilder.Entity("Web.Domain.AnnualVacation", b =>
                {
                    b.HasOne("Web.Domain.Worker", "Worker")
                        .WithMany("AnnualVacations")
                        .HasForeignKey("WorkerId1");
                });

            modelBuilder.Entity("Web.Domain.Assignment", b =>
                {
                    b.HasOne("Web.Domain.WorkingDay", "WorkingDay")
                        .WithMany("Assignments")
                        .HasForeignKey("WorkingDayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Domain.BankHoliday", b =>
                {
                    b.HasOne("Web.Domain.Team", "Team")
                        .WithMany("BankHolidays")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Domain.Worker", b =>
                {
                    b.HasOne("Web.Domain.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Domain.WorkingDay", b =>
                {
                    b.HasOne("Web.Domain.Worker")
                        .WithMany("WorkingDays")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
