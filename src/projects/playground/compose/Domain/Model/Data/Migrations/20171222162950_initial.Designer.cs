﻿// <auto-generated />
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Domain.Model.Data.Migrations
{
    [DbContext(typeof(ModelDbContext))]
    [Migration("20171222162950_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Model.Topic", b =>
                {
                    b.Property<string>("HashTag")
                        .HasColumnType("varchar(128)");

                    b.Property<Guid>("CreateCommandId");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("HashTag")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("Topics","model");
                });
#pragma warning restore 612, 618
        }
    }
}
